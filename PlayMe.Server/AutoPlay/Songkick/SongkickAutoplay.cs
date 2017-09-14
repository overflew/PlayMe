using Nerdle.AutoConfig;
using PlayMe.Common.Model;
using PlayMe.Data;
using PlayMe.Plumbing.Diagnostics;
using PlayMe.Server.AutoPlay.Meta;
using PlayMe.Server.AutoPlay.Songkick.SongkickApi;
using PlayMe.Server.AutoPlay.Util;
using PlayMe.Server.Providers.NewSpotifyProvider;
using PlayMe.Server.Providers.NewSpotifyProvider.Mappers;
using SpotifyAPI.Web.Models;
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
        private readonly SongkickApiService _songkickApiService;
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
            SongkickApiService songkickApiService,
            NewSpotifyProvider spotify,
            IDataService<QueuedTrack> queuedTrackDataService,
            ITrackMapper trackMapper,
            ILogger logger
            )
        {
            _songkickApiService = songkickApiService;
            _spotify = spotify;
            _queuedTrackDataService = queuedTrackDataService;
            _trackMapper = trackMapper;
            _logger = logger;

            _songkickConfig = AutoConfig.Map<ISongkickConfig>();

            _random = new Random();
        }

        public QueuedTrack FindTrack()
        {
            var upcomingEvents = _songkickApiService.GetUpcomingEvents(_songkickConfig.RegionId);

            FullArtist artistResult = null;
            Event eventDetail = null;

            for (int retries = 3; retries > 0; retries--)
            {
                var randomGig = PickRandomGig(upcomingEvents);

                // TODO: Be able to pick from all headline/support artists
                var firstArtist = randomGig.Performance.First();

                var result = FindSpotifyArtist(firstArtist);

                if (result != null) {
                    artistResult = result;
                    eventDetail = randomGig;
                    break;
                }
            }

            if (artistResult == null)
            {
                throw new AutoplayBlockedException("Couldn't find a matched artist");
            }

            var track = PickTrackByArtist(artistResult.Id);

            var mappedTrack = MapTrack(track, eventDetail);

            return mappedTrack;
        }

        private Event PickRandomGig(UpcomingEventsResponse upcomingEvents)
        {
            var events = upcomingEvents.ResultsPage.Results.Event;

            var randomIndex = _random.Next(events.Count());
            return events.ToArray()[randomIndex];
        }

        private FullArtist FindSpotifyArtist(SongkickApi.Artist artist)
        {
            var spotifyClient = _spotify.GetClient();

            var artistSearchResults = spotifyClient.SearchItems(
                artist.DisplayName, 
                SpotifyAPI.Web.Enums.SearchType.Artist, 
                market: "NZ");

            if (artistSearchResults.Artists.Total == 0)
            {
                return null;
            }

            var firstArtistResult = artistSearchResults.Artists.Items.First();

            if (string.Equals(firstArtistResult.Name, artist.DisplayName, StringComparison.InvariantCultureIgnoreCase)) {
                return firstArtistResult;
            }

            _logger.Debug($"Artist match bot close enough: Searched: '{artist.DisplayName}', spotify result: {firstArtistResult.Name}");

            return null;
        }

        private FullTrack PickTrackByArtist(string artistId)
        {
            var client = _spotify.GetClient();
            var artistTopTracks = client.GetArtistsTopTracks(artistId, "NZ");

            if (!artistTopTracks.Tracks.Any())
            {
                return null;
            }

            var randomIndex = _random.Next(artistTopTracks.Tracks.Count());
            return artistTopTracks.Tracks.ToArray()[randomIndex];
        }
        
        private QueuedTrack MapTrack(FullTrack track, Event eventDetail)
        {
            // -- Map it to the business models
            var mappedTrack = _trackMapper.Map(track, AutoplayDisplayName, true, true);

            var result = new QueuedTrack()
            {
                Track = mappedTrack,

                // Important: Tag the source of this autoplay variant
                User = AutoplayDisplayName,
                Reason = $"Gig: {eventDetail.DisplayName}"
            };

            result.AutoplayMetaInfo = new AutoplayMetaInfo()
            {
                AutoplayNameId = AnalysisId
            };

            return result;
        }
    }
}
