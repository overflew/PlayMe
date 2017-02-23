using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayMe.Common.Model;
using PlayMe.Common.Util;
using PlayMe.Data;
using PlayMe.Data.Mongo;
using PlayMe.Server.Providers.NewSpotifyProvider;
using PlayMe.Server.Providers.NewSpotifyProvider.Mappers;

namespace PlayMe.Server.AutoPlay.Recommendations
{
    public class RecommendationsAutoplay : IAutoPlay
    {
        private readonly NewSpotifyProvider _spotify;
        private readonly IDataService<QueuedTrack> _queuedTrackDataService;
        private readonly ITrackMapper _trackMapper;

        private readonly Random _random;

        const string AutoplayDisplayName = "Autoplay - Recommendations";

        private const int SEED_TRACKS = 30;

        // TODO: Move this out to external config. It's required in searches in order to populate 'IsPlayable' on tracks
        private const string LOCAL_MARKET = "NZ";

        public RecommendationsAutoplay(
            // TODO: Inject these via interfaces
            NewSpotifyProvider spotify,
            IDataService<QueuedTrack> queuedTrackDataService,
            ITrackMapper trackMapper
            )
        {
            _spotify = spotify;
            _queuedTrackDataService = queuedTrackDataService;
            _trackMapper = trackMapper;

            _random = new Random();
        }

        public QueuedTrack FindTrack()
        {
            // -- Pick a random seed track
            var seedTracks = PickRandomSeedTracks();

            // -- Grab recommendations
            var client = _spotify.GetClient();
            var recommendations = client.GetRecommendations(
                trackSeed: seedTracks.Select(s => s.Track.Link).ToList(), 
                market: LOCAL_MARKET);

            var pickIndex = _random.Next(recommendations.Tracks.Count);
            var chosenTrack = recommendations.Tracks[pickIndex];

            // (Temp) - get full track, to please the mapper...
            var chosenTrackFull = client.GetTrack(chosenTrack.Id);

            // -- Map it to the business models
            var mappedTrack = _trackMapper.Map(chosenTrackFull, AutoplayDisplayName, true, true);

            var seedString = string.Join(",", seedTracks.Select(s => $"{s.Track.Artists.First().Name} - {s.Track.Name}"));
            return new QueuedTrack()
            {
                Track = mappedTrack,

                // Important: Tag the source of this autoplay variant
                User = AutoplayDisplayName,
                Reason = $"Seed: {seedString}"
            };
        }

        private IList<QueuedTrack> PickRandomSeedTracks()
        {
            var recentTracks = PickSeedableTracksFromHistory();

            recentTracks.Shuffle();

            var result = new List<QueuedTrack>();
            var rnd = _random.Next(2);
            var tracksToPick = rnd == 0 
                ? 1 // 50% chance of just using one
                : _random.Next(2, 5); // Or randomly use more

            return recentTracks.Take(tracksToPick).ToList();
        }

        private IList<QueuedTrack> PickSeedableTracksFromHistory()
        {
            var x = _queuedTrackDataService.GetAll()
                .Where(t => t.Likes.Any())
                .OrderByDescending(t => t.StartedPlayingDateTime)
                .Take(SEED_TRACKS)
                .ToList();

            return x;
        }

    }
}
