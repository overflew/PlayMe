using System;
using PlayMe.Common.Model;
using SpotifyAPI.Web.Models;

namespace PlayMe.Server.Providers.NewSpotifyProvider.Mappers
{

    public interface IAlbumMapper
    {
        Album Map(
            SimpleAlbum album, 
            //SpotifyMusicProvider musicProvider, 
            bool mapArtist = false
            );
    }

    public class AlbumMapper : IAlbumMapper
    {
        private readonly IArtistMapper artistMapper;
        
        public AlbumMapper(IArtistMapper artistMapper)
        {
            this.artistMapper = artistMapper;
        }

        public Album Map(
            SimpleAlbum album, 
            //SpotifyMusicProvider musicProvider, 
            bool mapArtist = false)
        {
            var albumResult = new Album
            {
                Link = album.Id,
                Name = album.Name,
                //Year = album..Year,
                //ArtworkId = album.CoverId,
                ArtworkUrlLarge = album.Images[0].Url, // TODO: Ensure sort order on images?
                ArtworkUrlMedium = album.Images[1].Url, // TODO: Check array size, to stop crashes
                ArtworkUrlSmall = album.Images[2].Url,
                IsAvailable = true, //album.IsAvailable,
                //MusicProvider = musicProvider.Descriptor,
                ExternalLink = new Uri(album.ExternalUrls["spotify"])
            };

            return albumResult;

        }
    }
}