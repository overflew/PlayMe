using PlayMe.Common.Model;
using PlayMe.Data;
using PlayMe.Data.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Server.Queue
{
    public interface IForbiddenMusicService
    {
        TrackBlockedReason CanQueueTrack(QueuedTrack track);
    }

    /** Rules for if a song can be added to the queue.
        To avoid repeating a song too early, etc.
    */
    public class ForbiddenMusicService: IForbiddenMusicService
    {
        private readonly IDataService<MapReduceResult<TrackScore>> trackScoreDataService;
        private readonly IDataService<QueuedTrack> queuedTrackDataService;
        private readonly Settings settings;

        public ForbiddenMusicService(
            IDataService<MapReduceResult<TrackScore>> trackScoreDataService,
            IDataService<QueuedTrack> queuedTrackDataService,
            Settings settings

            )
        {
            this.trackScoreDataService = trackScoreDataService;
            this.queuedTrackDataService = queuedTrackDataService;
            this.settings = settings;
        }

        public TrackBlockedReason CanQueueTrack (QueuedTrack track)
        {
            var checkTrackScores = TooRecentlyPlayedRule_CheckTrackScores(track);
            if (checkTrackScores != null)
            {
                return checkTrackScores;
            }

            var checkTrackHistory = TrackHistoryRules(track);
            if (checkTrackHistory != null)
            {
                return checkTrackHistory;
            }

            return null;
        }

        private TrackBlockedReason TooRecentlyPlayedRule_CheckTrackScores(QueuedTrack track)
        {
            if (settings.DontRepeatTrackForHours <= 0)
            {
                return null;
            }

            var minTimeSpanSinceLastPlay = TimeSpan.FromHours(settings.DontRepeatTrackForHours);
            
            // Get Autoplay DB
            var x = trackScoreDataService
                .GetAll()
                .Where(s => s.value.Track.Link == track.Track.Link)
                .FirstOrDefault();

            if (x != null
                && x.value.MillisecondsSinceLastPlay < minTimeSpanSinceLastPlay.TotalMilliseconds)
            {
                return new TrackBlockedReason("Played too recently (Track scores)");
            }

            return null;
        }

        private TrackBlockedReason TrackHistoryRules(QueuedTrack track)
        {
            var recentlyPlayedInstances = queuedTrackDataService.GetAll()
                .Where(t => t.Track.Link == track.Track.Link)
                .OrderByDescending(t => t.StartedPlayingDateTime)
                .Take(5)
                .ToList();

            if (!recentlyPlayedInstances.Any())
            {
                return null;
            }

            var playedTooRecently = TrackHistoryRules_PlayedTooRecently(recentlyPlayedInstances);
            if (playedTooRecently != null)
            {
                return playedTooRecently;
            }

            return null;
        }

        private TrackBlockedReason TrackHistoryRules_PlayedTooRecently(IList<QueuedTrack> recentlyPlayedInstances)
        {
            if (settings.DontRepeatTrackForHours <= 0)
            {
                return null;
            }

            var mustHaveLastPlayedBefore = DateTime.Now.AddHours(-settings.DontRepeatTrackForHours);

            if (recentlyPlayedInstances
                   .Where(t => t.StartedPlayingDateTime > mustHaveLastPlayedBefore)
                   .Any())
            {
                return new TrackBlockedReason("Played too recently (Track history)");
            }

            return null;
        }
    }
}
