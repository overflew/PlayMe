using System;
using System.Collections.Generic;
using PlayMe.Common.Model;
using PlayMe.Server.AutoPlay.MultiAutoPlay.Config;
using PlayMe.Common.Util;
using PlayMe.Server.AutoPlay.MultiAutoPlay;
using System.Threading;
using PlayMe.Plumbing.Diagnostics;
using PlayMe.Server.Providers.NewSpotifyProvider;
using PlayMe.Server.Queue;
using System.Linq;
using PlayMe.Server.AutoPlay.Util;

namespace PlayMe.Server.AutoPlay.MultiAutoplay
{
    public class MultiAutoPlay : IAutoPlay
    {
        private readonly Stack<QueuedTrack> _tracksForAutoplaying = new Stack<QueuedTrack>();

        private readonly IList<IWeightedAutoPlay> autoPlayRepository;
        private readonly AutoPlayResolver autoPlayResolver;
        private readonly NewSpotifyProvider _spotify;
        private readonly IForbiddenMusicService _forbiddenMusicService;
        private readonly ILogger _logger;

        // Set to a reasonable number to handle veto-battles
        private const int TRACK_CACHE_SIZE = 5;
        private const int GET_TRACK_RETRY_LIMIT = 15;

        private static Object _fillCacheLock = new Object();
        private static Object _querySongsLock = new Object();

        public MultiAutoPlay(
            IWeightedAutoPlayRepository autoPlayRepository,
            AutoPlayResolver autoPlayResolver,
            NewSpotifyProvider spotify,
            IForbiddenMusicService forbiddenMusicService,
            ILogger logger)
        {
            _logger = logger;
            _spotify = spotify;
            _forbiddenMusicService = forbiddenMusicService;

            this.autoPlayRepository = autoPlayRepository.GetAllAutoPlays();

            if (this.autoPlayRepository.Count == 0)
            {
                throw new Exception("No autoPlay instances loaded by WeightedAutoPlayProvider.");
            }

            this.autoPlayResolver = autoPlayResolver;
        }

        public QueuedTrack FindTrack()
        {
            if (_tracksForAutoplaying.Count == 0)
            {
                FillCacheAsync();
                return GetOneRandomTrack();
            }

            FillCacheAsync(); // Keep cache topped up
            return _tracksForAutoplaying.Pop();
        }

        private void FillCacheAsync()
        {
            lock (_fillCacheLock)
            {
                ThreadPool.QueueUserWorkItem(FillCache);
            }
        }

        private void FillCache(object stateInfo)
        {
            var numTrackToGet = TRACK_CACHE_SIZE - _tracksForAutoplaying.Count;

            for (int i = 0; i < numTrackToGet; i++)
            {
                var track = GetOneRandomTrack();

                if (track == null)
                {
                    continue;
                }

                _tracksForAutoplaying.Push(track);
            }
        }

        private bool isAlreadyQueued(QueuedTrack track)
        {
            return _tracksForAutoplaying.ToList()
                .Where(t => t.Track.Link == track.Track.Link)
                .Any();
        }

        private QueuedTrack GetOneRandomTrack()
        {
            for (int i = 0; i < GET_TRACK_RETRY_LIMIT; i++)
            {
                try {
                    // 1) pick random AutoPlay
                    var autoPlay = WeightingUtil.ChooseWeightedRandom(autoPlayRepository);

                    // 2) get instance of selected AutoPlay
                    var instance = autoPlayResolver.GetAutoPlayInstance(autoPlay.Name);

                    // 3) queue new track
                    lock (_querySongsLock) // Don't multi-request from our async thread
                    {
                        var track = instance.FindTrack();

                        var canQueueTrack = _forbiddenMusicService.CanQueueTrack(track);
                        if (canQueueTrack != null) {
                            _logger.Info($"! - Not queueing track: {track.Track.Name}. Reason: {canQueueTrack.Reason}");
                            continue;
                        }

                        if (isAlreadyQueued(track))
                        {
                            _logger.Info($"! - Not queueing track: {track.Track.Name}. Reason: It's already in the autoplay cache");
                            continue;
                        }

                        return track;
                    }
                }
                catch (NewSpotifyApiException ex)
                {
                    if (ex.SpotifyClientError.Status == 403)
                    {
                        _logger.Warn($"[Multi autoplay] - Resetting auth token: {ex.Message}");
                        _spotify.ResetCachedAuth();
                        continue;
                    }

                    _logger.Error($"[Multi autoplay] - Error getting track: {ex.Message} | {ex.SpotifyClientError.Message}");                    
                }
                catch(AutoplayBlockedException ex)
                {
                    // These are more tolerable - so just log as warning.
                    _logger.Warn($"[Multi autoplay] {ex.Message}");
                }
                catch (Exception ex)
                {
                    _logger.ErrorException($"[Multi autoplay] - Error getting track", ex);
                }
            }

            // TODO: How to get this to sleep for 1 min, then start again?
            var fatalMessage = "[Multi autoplay] Failed to load any tracks. See earlier errors for detail. Stopping.";
            _logger.Fatal(fatalMessage);
            throw new Exception(fatalMessage);
        }
    }
}
