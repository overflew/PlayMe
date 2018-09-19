using System;
using PlayMe.Common.Model;
using SpotifyAPI.Web.Models;
using PlayMe.Server.Providers.SpotifyProvider;
using System.Linq;

namespace PlayMe.Server.Providers.NewSpotifyProvider.Mappers
{

    public interface IAlbumMapper
    {
        Album Map(
            SimpleAlbum album, 
            bool mapArtist = false
            );

        Album Map(
            FullAlbum album,
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
            bool mapArtist = false)
        {
            var albumResult = new Album
            {
                Link = album.Id,
                Name = album.Name,
                //Year = // TODO: Process this off album.ReleaseDate
                //ArtworkId = album.CoverId,
                IsAvailable = true, //album.IsAvailable,
                MusicProvider = SpotifyConsts.SpotifyMusicProviderDescriptor,
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

        public Album Map(
            FullAlbum album,
            bool mapArtist = false)
        {
            var albumResult = new Album
            {
                Link = album.Id,
                Name = album.Name,
                //Year = album.Year, // TODO: Process this off album.ReleaseDate
                //ArtworkId = album.CoverId,
                IsAvailable = true, //album.IsAvailable,
                MusicProvider = SpotifyConsts.SpotifyMusicProviderDescriptor,
                ExternalLink = new Uri(album.ExternalUrls["spotify"]),
                
            };

            if (mapArtist)
            {
                albumResult.Artist = artistMapper.Map(album.Artists.First());
            }


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