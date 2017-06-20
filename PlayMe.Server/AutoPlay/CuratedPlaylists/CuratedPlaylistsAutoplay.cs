using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayMe.Common.Model;
using PlayMe.Plumbing.Diagnostics;
using PlayMe.Server.Providers.NewSpotifyProvider;
using PlayMe.Server.Providers.NewSpotifyProvider.Mappers;
using SpotifyAPI.Web.Models;

namespace PlayMe.Server.AutoPlay.CuratedPlaylists
{
    public class CuratedPlaylistsAutoplay : IStandAloneAutoPlay
    {
        // User name to appear in the UI
        const string CuratedPlaylistsDisplayName = "Autoplay - Curated playlists";
        const string LoggingPrefix = "[CuratedPlaylists]";

        // TODO: Move this out to external config. It's required in searches in order to populate 'IsPlayable' on tracks
        private const string LOCAL_MARKET = "NZ";
        private const int GET_PLAYABLE_TRACK_RETY_LIMIT = 5;

        private readonly Random _random;

        private readonly NewSpotifyProvider _spotify;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ILogger _logger;
        private readonly ITrackMapper _trackMapper;

        public CuratedPlaylistsAutoplay(
            // TODO: Inject these via interfaces
            NewSpotifyProvider spotify,
            PlaylistRepository playlistRepository,
            ILogger logger,
            ITrackMapper trackMapper)
        {
            _trackMapper = trackMapper;
            _spotify = spotify;
            _playlistRepository = playlistRepository;
            _logger = logger;

            _random = new Random();
        }

        public QueuedTrack FindTrack()
        {
            // -- Source a playlist

            var playlistConfig = _playlistRepository.GetRandomPlaylist();

            var client = _spotify.GetClient();
            
            var playlist = client.GetPlaylist(playlistConfig.User, playlistConfig.PlaylistId, null, LOCAL_MARKET);
            if (playlist.HasError())
            {
                throw new NewSpotifyApiException(
                    $"{LoggingPrefix} Unable to load playlist: (User: '{playlistConfig.User}', playlist: '{playlistConfig.PlaylistName}' / {playlistConfig.PlaylistId}",
                    playlist.Error);
            }

            // -- Pick a random song

            var randomTrack = PickRandomTrackThatsPlayable(playlist);

            if (randomTrack == null)
            {
                // Panic mode? Hope that the core system will just move on & try something else...
                return null;
            }

            // -- Map it to the business models
            var mappedTrack = _trackMapper.Map(randomTrack.Track, CuratedPlaylistsDisplayName, true, true);
            
            return new QueuedTrack()
            {
                Track = mappedTrack,

                // Important: Tag the source of this autoplay variant
                User = CuratedPlaylistsDisplayName,
                Reason = $"Playlist: {playlist.Name}"
            };

        }

        private PlaylistTrack PickRandomTrackThatsPlayable(FullPlaylist playlist)
        {
            // TODO: This, but better?
            // Have a limited # of attempts at get a song that registers as being 'IsPlayable' (as a few aren't...)
            for (int i = 0; i < GET_PLAYABLE_TRACK_RETY_LIMIT; i++)
            {
                var track = PickRandomTrack(playlist);
                if (track.Track.IsPlayable.GetValueOrDefault())
                {
                    return track;
                }

                _logger.Debug($"Skipping unplayable track: {track.Track.Name} - {track.Track.Artists.First().Name}" );
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
                    $"{LoggingPrefix} Unable to load playlist: (User: '{playlist.Owner.DisplayName}', playlist id: '{playlist.Name}' / {playlist.Id}, page: {page})",                                    
                    paginatedTracks.Error);
            }

            var randomTrack = paginatedTracks.Items[indexAfterOffset];

            return randomTrack;
        }
    }
}
