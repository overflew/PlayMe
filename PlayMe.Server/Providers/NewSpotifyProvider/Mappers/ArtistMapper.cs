using System;
using System.Linq;
using PlayMe.Common.Model;
using SpotifyAPI.Web.Models;
using PlayMe.Server.Providers.SpotifyProvider;

namespace PlayMe.Server.Providers.NewSpotifyProvider.Mappers
{
    public interface IArtistMapper
    {
        Artist Map(
            SimpleArtist artist
            );

        Artist Map(
            FullArtist artist
            );
    }

    public class ArtistMapper : IArtistMapper
    {
        public Artist Map(
            SimpleArtist artist
            )
        {
            var artistResult = new Artist
            {
                Link = artist.Id,
                Name = artist.Name,
                //PortraitId = artist.PortraitId,
                MusicProvider = SpotifyConsts.SpotifyMusicProviderDescriptor,
                ExternalLink = new Uri(artist.ExternalUrls["spotify"])
            };

            return artistResult;

        }

        public Artist Map(
            FullArtist artist
            )
        {
            var artistResult = new Artist
            {
                Link = artist.Id,
                Name = artist.Name,
                //PortraitId = artist.PortraitId,
                MusicProvider = SpotifyConsts.SpotifyMusicProviderDescriptor,
                ExternalLink = new Uri(artist.ExternalUrls["spotify"])
            };

            if (artist.Images != null)
            {
                if (artist.Images.Count >= 1)
                {
                    artistResult.PortraitUrlLarge = artist.Images[0].Url;
                }

                if (artist.Images.Count >= 2)
                {
                    artistResult.PortraitUrlMedium = artist.Images[1].Url;
                }

                if (artist.Images.Count >= 3)
                {
                    artistResult.PortraitUrlSmall = artist.Images[2].Url;
                }
            }            

            return artistResult;

        }
    }
}