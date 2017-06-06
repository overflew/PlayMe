using System;
using System.Collections.Generic;
using PlayMe.Common.Model;
using PlayMe.Server.AutoPlay.MultiAutoPlay.Config;
using PlayMe.Common.Util;
using PlayMe.Server.AutoPlay.MultiAutoPlay;
using System.Threading;

namespace PlayMe.Server.AutoPlay.MultiAutoplay
{
    public class MultiAutoPlay : IAutoPlay
    {
        private readonly Stack<QueuedTrack> _tracksForAutoplaying = new Stack<QueuedTrack>();

        IList<IWeightedAutoPlay> autoPlayRepository;
        AutoPlayResolver autoPlayResolver;

        // Set to a reasonable number to handle veto-battles
        private const int TRACK_CACHE_SIZE = 5;
        private static Object _fillCacheLock = new Object();
        private static Object _querySongsLock = new object();

        public MultiAutoPlay(IWeightedAutoPlayRepository autoPlayRepository, AutoPlayResolver autoPlayResolver)
        {
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

        private QueuedTrack GetOneRandomTrack()
        {
            // 1) pick random AutoPlay
            var autoPlay = WeightingUtil.ChooseWeightedRandom(autoPlayRepository);

            // 2) get instance of selected AutoPlay
            var instance = autoPlayResolver.GetAutoPlayInstance(autoPlay.Name);

            // 3) queue new track
            lock (_querySongsLock) // Don't multi-request from our async thread
            {
                return instance.FindTrack();
            }
        }
    }
}
