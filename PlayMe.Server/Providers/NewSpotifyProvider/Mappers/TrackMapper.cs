using System;
using System.Linq;
using PlayMe.Common.Model;
using PlayMe.Server.Helpers.Interfaces;
using PlayMe.Server.Providers.SpotifyProvider;

namespace PlayMe.Server.Providers.NewSpotifyProvider.Mappers
{
    public interface ITrackMapper
    {
        Track Map(
            SpotifyAPI.Web.Models.FullTrack track,
            string user,
            bool mapAlbum = true,
            bool mapArtists = true);

        Track Map(
           SpotifyAPI.Web.Models.SimpleTrack track,
           string user,
           bool mapAlbum = true);
    }

    public class TrackMapper : ITrackMapper
    {
        private readonly IAlreadyQueuedHelper alreadyQueuedHelper;
        private readonly IAlbumMapper albumMapper;
        private readonly IArtistMapper artistMapper;

        public TrackMapper(
            IAlbumMapper albumMapper, 
            IArtistMapper artistMapper, 
            IAlreadyQueuedHelper alreadyQueuedHelper)
        {
            this.artistMapper = artistMapper;
            this.albumMapper = albumMapper;
            this.alreadyQueuedHelper = alreadyQueuedHelper;
        }

        public Track Map(
            SpotifyAPI.Web.Models.FullTrack track, 
            string user, 
            bool mapAlbum = true,
            bool mapArtists = true)
        {
            var trackResult = new Track
            {
                Link = track.Id,
                Name = track.Name,
                IsAvailable = track.IsPlayable == null ? true : track.IsPlayable.Value,
                Duration = new TimeSpan(track.DurationMs),
                DurationMilliseconds = track.DurationMs,
                MusicProvider = SpotifyConsts.SpotifyMusicProviderDescriptor,
                ExternalLink = new Uri(track.ExternUrls["spotify"]),
                SongPreviewUrl = track.PreviewUrl
            };

            if (mapAlbum && track.Album != null)
            {
                trackResult.Album = albumMapper.Map(track.Album);
            }

            if (mapArtists && track.Artists != null)
            {
                trackResult.Artists = track.Artists.Select(t => artistMapper.Map(t)).ToArray();
            }

            //We want to set whether the track is already queued 
            if (alreadyQueuedHelper != null)
            {
                trackResult = alreadyQueuedHelper.ResetAlreadyQueued((trackResult), user);
            }

            return trackResult;
        }

        public Track Map(
            SpotifyAPI.Web.Models.SimpleTrack track,
            string user,
            bool mapArtists = true)
        {

            var trackResult = new Track
            {
                Link = track.Id,
                Name = track.Name,
                //IsAvailable = track.IsPlayable == null ? true : track.IsPlayable.Value,
                Duration = new TimeSpan(track.DurationMs),
                DurationMilliseconds = track.DurationMs,
                MusicProvider = SpotifyConsts.SpotifyMusicProviderDescriptor,
                ExternalLink = new Uri(track.ExternUrls["spotify"]),
                SongPreviewUrl = track.PreviewUrl
            };

            if (mapArtists && track.Artists != null)
            {
                trackResult.Artists = track.Artists.Select(t => artistMapper.Map(t)).ToArray();
            }

            //We want to set whether the track is already queued 
            if (alreadyQueuedHelper != null)
            {
                trackResult = alreadyQueuedHelper.ResetAlreadyQueued((trackResult), user);
            }

            return trackResult;
        }
    }
}