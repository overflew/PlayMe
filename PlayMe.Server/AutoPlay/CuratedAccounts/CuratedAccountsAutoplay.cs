﻿using System;
using System.Linq;
using PlayMe.Common.Model;
using PlayMe.Plumbing.Diagnostics;
using PlayMe.Server.AutoPlay.CuratedPlaylists;
using PlayMe.Server.Providers.NewSpotifyProvider;
using PlayMe.Server.Providers.NewSpotifyProvider.Mappers;
using SpotifyAPI.Web.Models;
using PlayMe.Server.AutoPlay.Meta;

namespace PlayMe.Server.AutoPlay.CuratedAccounts
{
    public class CuratedAccountsAutoplay : IStandAloneAutoPlay
    {
        private readonly NewSpotifyProvider _spotify;
        private readonly FollowedAccountsRepository _followedAccountsRepository;
        private readonly ILogger _logger;
        private readonly ITrackMapper _trackMapper;

        const string CuratedPlaylistsDisplayName = Constants.AutoplayUserNameBasePrefix + " - Curated accounts";
        const string LoggingPrefix = "[CuratedAccounts]";
        const string AnalysisId = "[CuratedAccounts]";

        // TODO: Move this out to external config. It's required in searches in order to populate 'IsPlayable' on tracks
        private const string LOCAL_MARKET = "NZ";

        private readonly Random _random;

        public CuratedAccountsAutoplay(
        // TODO: Inject these via interfaces
            NewSpotifyProvider spotify,
            FollowedAccountsRepository followedAccountsRepository,
            ILogger logger,
            ITrackMapper trackMapper)
        {
            _spotify = spotify;
            _followedAccountsRepository = followedAccountsRepository;
            _logger = logger;
            _trackMapper = trackMapper;

            _random = new Random();
        }

        public QueuedTrack FindTrack()
        {
            var client = _spotify.GetClient();

            // -- Source a user account

            var accountConfig = _followedAccountsRepository.GetRandomAccount();
            
            var fullPlaylist = PickRandomPlaylist(accountConfig.User);

            // -- Choose a song from the playlist

            var randomTrack = PickRandomTrackThatsPlayable(fullPlaylist);

            if (randomTrack == null)
            {
                // Panic mode? Hope that the core system will just move on & try something else...
                return null;
            }

            // -- Map it to the business models
            var mappedTrack = _trackMapper.Map(randomTrack.Track, CuratedPlaylistsDisplayName, true, true);

            var result = new QueuedTrack()
            {
                Track = mappedTrack,

                // Important: Tag the source of this autoplay variant
                User = CuratedPlaylistsDisplayName,
                Reason = $"Playlist: {accountConfig.DisplayName}: {fullPlaylist.Name}"
            };

            result.AutoplayMetaInfo = new AutoplayMetaInfo()
            {
                AutoplayNameId = AnalysisId,
                MetaInfo = new MetaInfo()
                {
                    PlaylistId = fullPlaylist.Id,
                    PlaylistName = fullPlaylist.Name,
                    AccountId = fullPlaylist.Owner.Id,
                    SpotifyUri = fullPlaylist.Uri,

                    AccountName = accountConfig.DisplayName
                }
            };

            return result;

        }

        private FullPlaylist PickRandomPlaylist(string user)
        {
            var client = _spotify.GetClient();
            
            var userPlaylists = client.GetUserPlaylists(user);
            if (userPlaylists.HasError())
            {
                throw new NewSpotifyApiException(
                    $"{LoggingPrefix} Unable to load user account: (User: '{user}')",
                    userPlaylists.Error);
            }

            // -- Choose a playlist
            var randomPlaylistIndex = _random.Next(userPlaylists.Total);
            var perPage = userPlaylists.Limit;
            var page = (int)(randomPlaylistIndex / perPage); // Floor rounding - via int

            var offset = page * perPage;

            if (page != 0)
            {
                // If not on the first page - Paginate
                
                userPlaylists = client.GetUserPlaylists(user, perPage, page);
                if (userPlaylists.HasError())
                {
                    throw new NewSpotifyApiException(
                        $"{LoggingPrefix} Unable to get user playlist page: (User: '{user}', page: {page})",
                        userPlaylists.Error);
                }
            }

            var randomPlaylist = userPlaylists.Items[randomPlaylistIndex - offset];

            var fullPlaylist = client.GetPlaylist(randomPlaylist.Owner.Id, randomPlaylist.Id, null, LOCAL_MARKET);
            if (fullPlaylist.HasError())
            {
                throw new NewSpotifyApiException(
                    $"{LoggingPrefix} Unable to get full playlist: (User: '{randomPlaylist.Owner.DisplayName}', playlist: '{randomPlaylist.Name}' / {randomPlaylist.Id})",
                    userPlaylists.Error);
            }

            return fullPlaylist;
        }

        // TODO: Move this code to some common library

        private PlaylistTrack PickRandomTrackThatsPlayable(FullPlaylist playlist)
        {
            // TODO: This, but better?
            // Have a limited # of attempts at get a song that registers as being 'IsPlayable' (as a few aren't...)
            for (int i = 0; i < 5; i++)
            {
                var track = PickRandomTrack(playlist);
                if (track.Track.IsPlayable.GetValueOrDefault())
                {
                    return track;
                }

                _logger.Debug($"Skipping unplayable track: {track.Track.Name} - {track.Track.Artists.First().Name}");
            }

            // Panic mode. Couldn't find a playable track.
            return null;
        }

        private PlaylistTrack PickRandomTrack(FullPlaylist playlist)
        {
            var randomIndex = _random.Next(playlist.Tracks.Total);
            var perPage = playlist.Tracks.Limit;

            var page = (int)(randomIndex / perPage); // Floor rounding - via int

            if (page == 0)
            {
                return playlist.Tracks.Items[randomIndex];
            }

            // If not on the first page - Paginate

            var offset = page * perPage;
            var indexAfterOffset = randomIndex - offset;

            var client = _spotify.GetClient();

            var paginatedTracks =
                client.GetPlaylistTracks(playlist.Owner.Id, playlist.Id, null, 100, offset, LOCAL_MARKET);
            if (paginatedTracks.HasError())
            {
                throw new NewSpotifyApiException(
                    $"{LoggingPrefix} Unable to load playlist: (User: '{playlist.Owner.DisplayName}', playlist: '{playlist.Name}' / {playlist.Id}, page: {page})",
                    paginatedTracks.Error);
            }

            var randomTrack = paginatedTracks.Items[indexAfterOffset];

            return randomTrack;
        }
    }
}
