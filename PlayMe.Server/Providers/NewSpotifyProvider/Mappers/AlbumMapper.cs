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
                IsAvailable = true, //album.IsAvailable,
                //MusicProvider = musicProvider.Descriptor,
                ExternalLink = new Uri(album.ExternalUrls["spotify"])
            };

            if (album.Images != null)
            {
                if (album.Images.Count >= 1)
                {
                    albumResult.ArtworkUrlLarge = album.Images[0].Url;
                }

                if (album.Images.Count >= 2)
                {
                    albumResult.ArtworkUrlMedium = album.Images[1].Url;
                }

                if (album.Images.Count >= 3)
                {
                    albumResult.ArtworkUrlSmall = album.Images[2].Url;
                }
            }

            return albumResult;

        }
    }
}