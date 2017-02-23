using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayMe.Common.Model;
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

            var recentTracks = PickSeedTracks();

            var rand = _random.Next(recentTracks.Count());
            var seedTrack = recentTracks[rand];

            // -- Grab recommendations
            var client = _spotify.GetClient();
            var recommendations = client.GetRecommendations(
                trackSeed: new List<string> {seedTrack.Track.Link}, 
                market: LOCAL_MARKET);

            var pickIndex = _random.Next(recommendations.Tracks.Count);
            var chosenTrack = recommendations.Tracks[pickIndex];

            // (Temp) - get full track, to please the mapper...
            var chosenTrackFull = client.GetTrack(chosenTrack.Id);

            // -- Map it to the business models
            var mappedTrack = _trackMapper.Map(chosenTrackFull, AutoplayDisplayName, true, true);

            return new QueuedTrack()
            {
                Track = mappedTrack,

                // Important: Tag the source of this autoplay variant
                User = AutoplayDisplayName,
                Reason = $"Seed: {seedTrack.Track.Artists.First().Name} - {seedTrack.Track.Name}"
            };
        }

        private IList<QueuedTrack> PickSeedTracks()
        {
            var x = _queuedTrackDataService.GetAll()
                .Where(t => t.Likes.Any())
                .OrderByDescending(t => t.StartedPlayingDateTime)
                .Take(10)
                .ToList();

            return x;
        }

    }
}
