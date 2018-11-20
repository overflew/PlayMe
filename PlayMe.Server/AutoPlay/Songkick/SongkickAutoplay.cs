﻿using Nerdle.AutoConfig;
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
        const bool DEBUG_GENRE_CHECK = false;

        private readonly SongkickApiService _songkickApiService;
        private readonly RandomGigPickerService _randomGigPickerService;
        private readonly SpotifyArtistMatcherService _spotifyArtistMatcherService;

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
            SpotifyArtistMatcherService spotifyArtistMatcherService,
            NewSpotifyProvider spotify,
            IDataService<QueuedTrack> queuedTrackDataService,
            ITrackMapper trackMapper,
            ILogger logger
            )
        {
            _songkickApiService = songkickApiService;
            _randomGigPickerService = randomGigPickerSerice;
            _spotifyArtistMatcherService = spotifyArtistMatcherService;

            _spotify = spotify;
            _queuedTrackDataService = queuedTrackDataService;
            _trackMapper = trackMapper;
            _logger = logger;

            _songkickConfig = AutoConfig.Map<ISongkickConfig>();

            _random = new Random();

            if (DEBUG_GENRE_CHECK)
            {
                debug_TestAllArtists();
            }
        }

        private UpcomingEventsResponse _cachedUpcomingEvents;
        private UpcomingEventsResponse GetUpcomingEvents()
        {
            if (_cachedUpcomingEvents == null)
            {
                // TODO: Actual cache w/expiry
                _cachedUpcomingEvents = _songkickApiService.GetUpcomingEvents(_songkickConfig.RegionId);
            }

            return _cachedUpcomingEvents;
        }        

        public QueuedTrack FindTrack()
        {
            var upcomingEvents = GetUpcomingEvents();

            //TODO status == error, no results, etc.
            if (!upcomingEvents.ResultsPage.Status.Equals("ok", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new AutoplayBlockedException($"Error getting Songkick results: {upcomingEvents.ResultsPage.Status}");
            }

            ArtistGigResult artistEventResult = null;
            FullArtist spotifyArtist = null;

            for (int retries = 10; retries > 0; retries--)
            {
                artistEventResult = _randomGigPickerService.PickRandomGig_ByArtistWeight(upcomingEvents.ResultsPage.Results.Event);

                spotifyArtist = _spotifyArtistMatcherService.FindSpotifyArtist(artistEventResult.ArtistName);

                if (spotifyArtist != null
                    && HasBadGenre(spotifyArtist))
                {                    
                    continue;
                }

                if (artistEventResult != null
                    && spotifyArtist != null)
                {
                    break;
                }
            }

            if (spotifyArtist == null)
            {
                var msg = artistEventResult == null
                    ? "Couldn't find a matched artist"
                    : $"Couldn't find a matched artist for '{artistEventResult.ArtistName}'";
                throw new AutoplayBlockedException(msg);
            }

            var spotifyTrack = _spotifyArtistMatcherService.PickTrackByArtist(spotifyArtist);

            // TODO: Handle null response
            if (spotifyTrack == null)
            {
                throw new AutoplayBlockedException($"Couldn't find a track for ${spotifyArtist.Name}");
            }

            var mappedTrack = MapTrack(spotifyTrack, artistEventResult.Event);

            // Debug: Learn more about genres...
            var genres = spotifyArtist.Genres != null && spotifyArtist.Genres.Any()
                ? string.Join(", ", spotifyArtist.Genres.ToList())
                : "(no genres)";
            mappedTrack.Reason += $" [${genres}]";
            _logger.Debug($"Genre finding: '{spotifyArtist.Name}': {genres}");

            mappedTrack.Reason += $", pop: {artistEventResult.Event.Popularity}";

            return mappedTrack;
        }
        
        private bool HasBadGenre(FullArtist artist)
        {
            if (artist.Genres == null
                || !artist.Genres.Any())
            {
                return false;
            }
                
            var badGenres = _songkickConfig.BadGenres.SelectMany(badGenre =>
                artist.Genres.Where(artistGenre =>
                    artistGenre.Contains(badGenre.Contains))).ToList();


            if (badGenres.Any())
            {
                _logger.Info($"Bad genre!: Skipping artist '{artist.Name}' - '{String.Join(", ", badGenres)}'");
                return true;
            }

            return false;
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

        private void debug_TestAllArtists() {
            var upcomingEvents = GetUpcomingEvents();

            //TODO status == error, no results, etc.
            if (!upcomingEvents.ResultsPage.Status.Equals("ok", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new AutoplayBlockedException("Error getting Songkick results");
            }

            _logger.Debug($"Songkick: {upcomingEvents.ResultsPage.Results.Event.Count()} events");

            upcomingEvents.ResultsPage.Results.Event.ForEach(e => {
                e.Performance.Where(p => p.Billing == "headline")                            
                            .Select(p => p.DisplayName)
                            .ToList()
                            .Distinct()
                            .ToList()
                            .ForEach(a =>
                {
                    var spotifyArtist = _spotifyArtistMatcherService.FindSpotifyArtist(a);
                    if (spotifyArtist == null)
                    {
                        //_logger.Debug($"SK: Artist: {a} -> Not found");
                        return;
                    }

                    var genres = spotifyArtist.Genres != null && spotifyArtist.Genres.Any()
                        ? string.Join(", ", spotifyArtist.Genres.ToList())
                        : "(no genres)";

                    _logger.Debug($"SK:\t{spotifyArtist.Name}\t{genres}\t{spotifyArtist.Popularity}\t{e.Popularity}");
                });
            });
        }
    }
}