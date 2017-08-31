using Nerdle.AutoConfig;
using PlayMe.Common.Model;
using PlayMe.Data;
using PlayMe.Plumbing.Diagnostics;
using PlayMe.Server.Providers.NewSpotifyProvider;
using PlayMe.Server.Providers.NewSpotifyProvider.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Server.AutoPlay.Songkick
{
    public class SongkickAutoplay : IStandAloneAutoPlay
    {
        private readonly NewSpotifyProvider _spotify;
        private readonly IDataService<QueuedTrack> _queuedTrackDataService;
        private readonly ITrackMapper _trackMapper;
        private readonly ILogger _logger;
        private readonly ISongkickConfig _songkickConfig;

        private readonly Random _random;

        const string AutoplayDisplayName = "Autoplay - Gigs";
        private const string LoggingPrefix = "[GigsAutoplay]";
        private const string AnalysisId = "[GigsAutoplay]";

        public SongkickAutoplay(
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

            _songkickConfig = AutoConfig.Map<ISongkickConfig>();

            _random = new Random();
        }

        public QueuedTrack FindTrack()
        {
            return null;
        }
    }
}
