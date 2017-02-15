using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayMe.Common.Model;
using PlayMe.Server.Providers.NewSpotifyProvider;

namespace PlayMe.Server.AutoPlay.CuratedPlaylists
{
    public class CuratedPlaylistsAutoplay : IAutoPlay
    {
        // User name to appear in the UI
        const string CuratedPlaylistsDisplayName = "Autoplay - Curated playlists";

        private readonly NewSpotifyProvider _spotify;
        private readonly IPlaylistRepository _playlistRepository;
        private readonly Random _random;

        public CuratedPlaylistsAutoplay(
            // TODO: Inject these via interfaces
            NewSpotifyProvider spotify,
            PlaylistRepository playlistRepository)
        {
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
            // TODO: For really large playlists, will we have to paginate to retrieve all the tracks?
            var randomIndex = _random.Next(playlist.Tracks.Items.Count);
            var randomTrack = playlist.Tracks.Items[randomIndex];

            // -- Map it to the business models
            // TODO: Create full mappers for track/artist/album
            var mappedTrack = new Track()
            {
                Link = randomTrack.Track.Id,
                Name = randomTrack.Track.Name
            };

            return new QueuedTrack()
            {
                Track = mappedTrack,

                // Important: Tag the source of this autoplay variant
                User = CuratedPlaylistsDisplayName,
                Reason = string.Format("Playlist: {0}", playlist.Name)
            };

        }
    }
}
