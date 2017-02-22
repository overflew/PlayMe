using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayMe.Common.Model;
using PlayMe.Server.Providers.NewSpotifyProvider;
using PlayMe.Server.Providers.NewSpotifyProvider.Mappers;
using SpotifyAPI.Web.Models;

namespace PlayMe.Server.AutoPlay.CuratedPlaylists
{
    public class CuratedPlaylistsAutoplay : IAutoPlay
    {
        // User name to appear in the UI
        const string CuratedPlaylistsDisplayName = "Autoplay - Curated playlists";

        private readonly Random _random;

        private readonly NewSpotifyProvider _spotify;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ITrackMapper _trackMapper;

        public CuratedPlaylistsAutoplay(
            // TODO: Inject these via interfaces
            NewSpotifyProvider spotify,
            PlaylistRepository playlistRepository,
            ITrackMapper trackMapper)
        {
            _trackMapper = trackMapper;
            _spotify = spotify;
            _playlistRepository = playlistRepository;

            _random = new Random();
        }

        public QueuedTrack FindTrack()
        {
            // -- Source a playlist

            var playlistConfig = _playlistRepository.GetRandomPlaylist();

            var client = _spotify.GetClient();
            
            var playlist = client.GetPlaylist(playlistConfig.User, playlistConfig.PlaylistId);

            // -- Pick a random song
            var randomTrack = PickRandomTrack(playlist);
            
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
            var paginatedTracks = client.GetPlaylistTracks(playlist.Owner.Id, playlist.Id, null, 100, offset);

            var randomTrack = paginatedTracks.Items[indexAfterOffset];

            return randomTrack;
        }
    }
}
