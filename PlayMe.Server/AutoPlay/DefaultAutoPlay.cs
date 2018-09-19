using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PlayMe.Common.Model;
using PlayMe.Data;
using PlayMe.Data.Mongo;
using PlayMe.Server.AutoPlay.TrackRandomizers;
using PlayMe.Server.Providers;
using System.Collections.Concurrent;

namespace PlayMe.Server.AutoPlay
{
    public class DefaultAutoPlay : IStandAloneAutoPlay
    {
        private readonly IDataService<MapReduceResult<TrackScore>> trackScoreDataService;
        private readonly IDataService<QueuedTrack> queuedTrackDataService;
        private readonly ConcurrentStack<QueuedTrack> _tracksForAutoplaying = new ConcurrentStack<QueuedTrack>();
        private readonly Settings settings;
        private readonly IMusicProviderFactory musicProviderFactory;
        private readonly IRandomizerFactory randomizerFactory;

        private const string DefaultAutoPlay_DisplayName = Constants.AutoplayUserNameBasePrefix;
        private const string AnalysisId = "[DefaultAutoplay]";

        public DefaultAutoPlay(
            IMusicProviderFactory musicProviderFactory,
            IDataService<MapReduceResult<TrackScore>> trackScoreDataService,
            IDataService<QueuedTrack> queuedTrackDataService,
            IRandomizerFactory randomizerFactory,
            Settings settings
            )
        {
            this.trackScoreDataService = trackScoreDataService;
            this.queuedTrackDataService = queuedTrackDataService;
            this.musicProviderFactory = musicProviderFactory;
            this.randomizerFactory = randomizerFactory;
            this.settings = settings;
        }

        public QueuedTrack FindTrack()
        {
            // Note: No longer will queue 'last track played' when the queue is empty.
            //       ^ As multi-autoplay will just get duplicates...

            if (_tracksForAutoplaying.Count <= settings.MinAutoplayableTracks)
            {
                FillBagWithAutoplayTracks(null);
                if (!_tracksForAutoplaying.Any())
                {
                    throw new Exception("Default autoplay could not load any tracks");
                }
            }

            QueuedTrack track = null;
            if (_tracksForAutoplaying.TryPop(out track))
            {
                randomizerFactory.Randomize.Execute(track);
            }

            // Reset this audit stuff. 
            // As it's picking up what was saved to the DB, there's a chance it would maybe have old audit data on it?
            track.AutoplayMetaInfo = new Meta.AutoplayMetaInfo()
            {
                AutoplayNameId = AnalysisId
            };

            return track;
        }

        private void FillBagWithAutoplayTracks(object stateInfo)
        {
            var scoredTracks = PickTracks();
            if (scoredTracks == null)
                return;

            var chosenTracks = scoredTracks;
            foreach (var qt in chosenTracks)
            {
                var queuedTrack = new QueuedTrack();
                var tracksMusicProvider = musicProviderFactory.GetMusicProviderByIdentifier(qt.value.Track.MusicProvider.Identifier);
                if (!tracksMusicProvider.IsEnabled) continue;
                var t = tracksMusicProvider.GetTrack(qt._id, DefaultAutoPlay_DisplayName);
                if (t == null) continue;
                queuedTrack.Track = t;
                queuedTrack.User = DefaultAutoPlay_DisplayName;

                _tracksForAutoplaying.Push(queuedTrack);
            }
        }

        private void FillBagWithLastTrack()
        {
            var chosenTrack = queuedTrackDataService.GetAll()
                .Where(t => !t.Vetoes.Any())
                .OrderByDescending(qt => qt.StartedPlayingDateTime)
                .FirstOrDefault();
            if (chosenTrack != null)
            {
                _tracksForAutoplaying.Push(chosenTrack);
            }
        }

        private IEnumerable<MapReduceResult<TrackScore>> PickTracks()
        {
            var minMillisecondsSinceLastPlay = TimeSpan.FromHours(settings.DontRepeatTrackForHours).TotalMilliseconds;
            var results = PickTracks(minMillisecondsSinceLastPlay);
            //If no tracks were returned, it may be that service has only just been installed. Don't restrict tracks based on last played time
            var mapReduceResults = results as IList<MapReduceResult<TrackScore>> ?? results.ToList();
            if (!mapReduceResults.Any()) results = PickTracks(0);
            return mapReduceResults;
        }

        private IEnumerable<MapReduceResult<TrackScore>> PickTracks(double minMillisecondsSinceLastPlay)
        {
            return trackScoreDataService.GetAll()
                .Where(s => !s.value.IsExcluded && s.value.MillisecondsSinceLastPlay > minMillisecondsSinceLastPlay)
                .OrderByDescending(s => s.value.Score)
                .Take(settings.MaxAutoplayableTracks)
                .ToList();
        }
    }
}

