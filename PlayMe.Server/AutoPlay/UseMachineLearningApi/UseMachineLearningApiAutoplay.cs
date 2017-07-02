using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayMe.Common.Model;
using PlayMe.Server.Providers.NewSpotifyProvider;
using PlayMe.Server.Providers.NewSpotifyProvider.Mappers;
using PlayMe.Plumbing.Diagnostics;

namespace PlayMe.Server.AutoPlay.UseMachineLearningApi
{
    public class UseMachineLearningApiAutoplay : IStandAloneAutoPlay
    {
        // User name to appear in the UI
        const string MachineLearningApiDisplayName = "Autoplay - Curated playlists";
        const string LoggingPrefix = "[ML]";
        const string AnalysisId = "[ML]";

        private readonly IMachineLearningApiService _machineLearningApiService;
        private readonly NewSpotifyProvider _spotify;
        private readonly ILogger _logger;
        private readonly ITrackMapper _trackMapper;

        private Random _random = new Random();

        public UseMachineLearningApiAutoplay(
            IMachineLearningApiService machineLearningApiService,
            NewSpotifyProvider spotify,
            ITrackMapper trackMapper,
            ILogger logger)
        {
            _machineLearningApiService = machineLearningApiService;

            _trackMapper = trackMapper;
            _spotify = spotify;
            _logger = logger;
        }

        public QueuedTrack FindTrack()
        {
            // Get seed & track from ML API
            var seedTrack = GetSeedTrack();

            // !! HELLO: Switch this to the 'GetSuggestionFromSeed' once you're ready
            var mlResult = _machineLearningApiService.DummyTest(seedTrack);

            // Load track data
            var client = _spotify.GetClient();
            var fullSong = client.GetTrack(mlResult.TrackId, "NZ");

            var mappedResult = _trackMapper.Map(fullSong, MachineLearningApiDisplayName,  true, true);

            var queuedTrack = new QueuedTrack()
            {
                Track = mappedResult,

                // Important: Tag the source of this autoplay variant
                User = MachineLearningApiDisplayName,
                Reason = $"Seed: {seedTrack.ArtistName} - {seedTrack.TrackName}"
            };

            return queuedTrack;
        }

        // -- TEMP: Some stuff to make testing easier

        private TrackSeed GetSeedTrack()
        {
            var randomIndex = _random.Next(DummyTrackSeeds.Count());
            return DummyTrackSeeds[randomIndex];
        }        

        private IList<TrackSeed> DummyTrackSeeds = new List<TrackSeed>() {
                new TrackSeed() {
                    TrackId = "2374M0fQpWi3dLnB54qaLX",
                    TrackName = "Africa",
                    ArtistName = "Toto"
                },
                new TrackSeed()
                {
                    TrackId = "0x38UMMhYacciPRK7ETk1M",
                    TrackName = "Can't keep checking my phone",
                    ArtistName = "Unknown Mortal Orchestra"
                },
                new TrackSeed()
                {
                    TrackId = "1xb5aS8n5nOlX5JHNXsPQE",
                    TrackName = "Redbone",
                    ArtistName = "Childish Gambino"
                },
                new TrackSeed()
                {
                    TrackId = "6y20BV5L33R8YXM0YuI38N",
                    TrackName = "No one knows",
                    ArtistName = "Queens of the Stone Age"
                },
                new TrackSeed()
                {
                    TrackId = "5ByAIlEEnxYdvpnezg7HTX",
                    TrackName = "Juicy",
                    ArtistName = "Notorious BIG"
                }
            };
    }
}
