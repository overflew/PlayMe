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
using PlayMe.Server.AutoPlay.Util;
using PlayMe.Plumbing.Diagnostics;
using Nerdle.AutoConfig;

namespace PlayMe.Server.AutoPlay.Recommendations
{
    public class RecommendationsAutoplay : IStandAloneAutoPlay
    {
        private readonly NewSpotifyProvider _spotify;
        private readonly IDataService<QueuedTrack> _queuedTrackDataService;
        private readonly ITrackMapper _trackMapper;
        private readonly ILogger _logger;
        private readonly IRecommendationsConfig _recommendationsConfig;

        private readonly Random _random;

        const string AutoplayDisplayName = "Autoplay - Recommendations";
        private const string LoggingPrefix = "[RecommendationsAutoplay]";
        
        // TODO: Move this out to external config. It's required in searches in order to populate 'IsPlayable' on tracks
        private const string LOCAL_MARKET = "NZ";

        public RecommendationsAutoplay(
            // TODO: Inject these via interfaces
            NewSpotifyProvider spotify,
            IDataService<QueuedTrack> queuedTrackDataService,
            ITrackMapper trackMapper,
            ILogger logger
            )
        {
            _spotify = spotify;
            _queuedTrackDataService = queuedTrackDataService;
            _trackMapper = trackMapper;
            _logger = logger;

            _recommendationsConfig = AutoConfig.Map<IRecommendationsConfig>();

            _random = new Random();
        }

        public QueuedTrack FindTrack()
        {
            // -- Pick a random seed track
            var seedTracks = PickRandomSeedTracks();
            if (seedTracks == null
                || !seedTracks.Any())
            {
                throw new AutoplayBlockedException("No seed tracks available for recommendations");
            }

            var firstTrack = seedTracks.First().Track;
            _logger.Debug($"Using seed track: {firstTrack.Name}, {firstTrack.Artists.First().Name}");

            // -- Grab recommendations
            var client = _spotify.GetClient();

            var recommendations = client.GetRecommendations(
                trackSeed: seedTracks.Select(s => s.Track.Link).ToList(), 
                market: LOCAL_MARKET);

            if (recommendations.HasError())
            {
                var firstSeed = seedTracks.FirstOrDefault();
                throw new NewSpotifyApiException(
                    $"{LoggingPrefix} Unable to load recommendations. Seed: {firstSeed.Track.Name}, {firstSeed.Track.Artists.First().Name}",
                    recommendations.Error);
            }
            if (!recommendations.Tracks.Any())
            {
                var firstSeed = seedTracks.FirstOrDefault();
                throw new AutoplayBlockedException($"{LoggingPrefix} No recommendations found for: {firstSeed.Track.Name}, {firstSeed.Track.Artists.First().Name} ");
            }

            var pickIndex = _random.Next(recommendations.Tracks.Count);

            _logger.Debug($"Picking track {pickIndex} of {recommendations.Tracks.Count}");
            var chosenTrack = recommendations.Tracks[pickIndex];

            // (Temp) - get full track, to please the mapper...
            var chosenTrackFull = client.GetTrack(chosenTrack.Id);
            if (chosenTrackFull.HasError())
            {
                throw new NewSpotifyApiException(
                    $"{LoggingPrefix} Unable to load track: {chosenTrack.Name}, {chosenTrack.Artists.First().Name}",
                    recommendations.Error);
            }

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

            var contentiousTracksToRemove =
                recentTracks.Where(t => t.Vetoes.Any()
                                        && (t.Likes.Count() / t.Vetoes.Count() < _recommendationsConfig.LikeToVetoSeedAcceptanceRatio)).ToList();
            if (contentiousTracksToRemove.Any())
            {
                _logger.Debug($"! Removed {contentiousTracksToRemove.Count()} tracks from the seeds");
                recentTracks = recentTracks.Except(contentiousTracksToRemove).ToList();
            }
            if (!recentTracks.Any())
            {
                throw new AutoplayBlockedException("No seeds remaining for recommendation");
            }

            recentTracks.Shuffle();

            return recentTracks.Take(1).ToList();

            // FUTURE: Get multiple seeds - based on artist genre.            
        }

        private IList<QueuedTrack> PickSeedableTracksFromHistory()
        {
            var userQueuedSeeds = PickSeedableTracksFromHistory_QueuedByUsers();
            var autoplayQueuedSeeds = PickSeedableTracksFromHistory_QueuedByAutoplay();

            var allSeeds = userQueuedSeeds.Union(autoplayQueuedSeeds);

            return allSeeds.ToList();
        }

        private IList<QueuedTrack> PickSeedableTracksFromHistory_QueuedByAutoplay()
        {
            var seedTracks = _queuedTrackDataService.GetAll()
                .Where(t => 
                    t.User.StartsWith("Autoplay")
                    && t.Likes.Any()
                    && !t.IsSkipped)
                .OrderByDescending(t => t.StartedPlayingDateTime)
                .Take(_recommendationsConfig.AutoplayPopularSeeds)
                .ToList();

            return seedTracks;
        }

        private IList<QueuedTrack> PickSeedableTracksFromHistory_QueuedByUsers()
        {
            var seedTracks = _queuedTrackDataService.GetAll()
                .Where(t => 
                    !t.User.StartsWith("Autoplay")
                    && !t.IsSkipped)
                .OrderByDescending(t => t.StartedPlayingDateTime)
                .Take(_recommendationsConfig.UserTracksSeeds)
                .ToList();

            return seedTracks;
        }

    }
}
