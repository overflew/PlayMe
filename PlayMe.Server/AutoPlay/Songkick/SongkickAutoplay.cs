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
        private readonly SongkickApiService _songkickApiService;
        private readonly RandomGigPickerService _randomGigPickerService;

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
            SongkickApiService songkickApiService,
            RandomGigPickerService randomGigPickerSerice,
            NewSpotifyProvider spotify,
            IDataService<QueuedTrack> queuedTrackDataService,
            ITrackMapper trackMapper,
            ILogger logger
            )
        {
            _songkickApiService = songkickApiService;
            _randomGigPickerService = randomGigPickerSerice;
           
            _spotify = spotify;
            _queuedTrackDataService = queuedTrackDataService;
            _trackMapper = trackMapper;
            _logger = logger;

            _songkickConfig = AutoConfig.Map<ISongkickConfig>();

            _random = new Random();
        }

        private UpcomingEventsResponse _cachedUpcomingEvents;
        private UpcomingEventsResponse GetUpcomingEvents()
        {
            if (_cachedUpcomingEvents == null)
            {
                _cachedUpcomingEvents = _songkickApiService.GetUpcomingEvents(_songkickConfig.RegionId);
            }

            return _cachedUpcomingEvents;
        }        

        public QueuedTrack FindTrack()
        {
            var upcomingEvents = GetUpcomingEvents();

            ArtistGigResult artistEventResult = null;
            FullArtist spotifyArtist = null;

            for (int retries = 3; retries > 0; retries--)
            {
                artistEventResult = _randomGigPickerService.PickRandomGig_ByArtistWeight(upcomingEvents.ResultsPage.Results.Event);

                spotifyArtist = FindSpotifyArtist(artistEventResult.ArtistName);

                if (artistEventResult != null
                    && spotifyArtist != null)
                {
                    break;
                }
            }

            if (artistEventResult == null)
            {
                throw new AutoplayBlockedException("Couldn't find a matched artist");
            }

            var spotifyTrack = PickTrackByArtist(spotifyArtist);

            var mappedTrack = MapTrack(spotifyTrack, artistEventResult.Event);

            return mappedTrack;
        }

        private FullArtist FindSpotifyArtist(string artistName)
        {
            // -- Find matching artist candidates from Spotify

            var spotifyClient = _spotify.GetClient();

            var searchableArtistName = GetSearchableArtistName(artistName);

            var artistSearchResults = spotifyClient.SearchItems(
                searchableArtistName, 
                SpotifyAPI.Web.Enums.SearchType.Artist, 
                market: "NZ");

            if (artistSearchResults.Artists.Total == 0)
            {
                return null;
            }

            // -- Pick a reasonably-exact match

            var compareOptions = System.Globalization.CompareOptions.IgnoreNonSpace // Make sure to allow mācrön-matches
                | System.Globalization.CompareOptions.IgnoreSymbols
                | System.Globalization.CompareOptions.IgnoreCase;

            var comparer = System.Globalization.CultureInfo.CurrentCulture.CompareInfo;

            var searchableArtistKey = MakeSearchKey(artistName);
            var firstArtistResult = artistSearchResults.Artists.Items
                .Where(a => comparer.Compare(MakeSearchKey(a.Name), searchableArtistKey, compareOptions) == 0)
                .FirstOrDefault();

            if (firstArtistResult == null)
            {
                _logger.Debug($"No artist match found for: '{artistName}'");
            }

            return firstArtistResult;            
        }

        private string MakeSearchKey(string s)
        {
            if (s.Contains("&"))
            {
                s = s.Replace("&", "and");
            }           

            return s;
        }

        private string GetSearchableArtistName(string artistName)
        {
            if (artistName.EndsWith(" NZ", StringComparison.InvariantCultureIgnoreCase))
            {
                artistName = artistName.Replace(" NZ", "");
            }

            if (artistName.EndsWith(" (NZ)", StringComparison.InvariantCultureIgnoreCase))
            {
                artistName = artistName.Replace(" (NZ)", "");                    
            }

            return artistName;              
        }

        private FullTrack PickTrackByArtist(FullArtist spotifyArtist)
        {
            var client = _spotify.GetClient();
            var artistTopTracks = client.GetArtistsTopTracks(spotifyArtist.Id, "NZ");

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
