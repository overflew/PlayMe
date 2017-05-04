using System;
using System.Linq;
using PlayMe.Common.Model;
using SpotifyAPI.Web.Models;

namespace PlayMe.Server.Providers.NewSpotifyProvider.Mappers
{
    public interface IArtistMapper
    {
        Artist Map(
            SimpleArtist artist
            //SpotifyMusicProvider musicProvider
            );

        Artist Map(
            FullArtist artist
            //SpotifyMusicProvider musicProvider
            );
    }

    public class ArtistMapper : IArtistMapper
    {
        public Artist Map(
            SimpleArtist artist
            //spotifyMusicProvider musicProvider
            )
        {
            //string artistLink = artist.GetLinkString();
            var artistResult = new Artist
            {
                Link = artist.Id,
                Name = artist.Name,
                //PortraitId = artist.PortraitId,
                //MusicProvider = musicProvider.Descriptor,
                ExternalLink = new Uri(artist.ExternalUrls["spotify"])
            };

            // TODO: Figure out how to not do this...
            artistResult.MusicProvider = new MusicProviderDescriptor()
            {
                Identifier = "sp",
                Name = "Spotify"
            };

            return artistResult;

        }

        public Artist Map(
            FullArtist artist
            //spotifyMusicProvider musicProvider
            )
        {
            //string artistLink = artist.GetLinkString();
            var artistResult = new Artist
            {
                Link = artist.Id,
                Name = artist.Name,
                //PortraitId = artist.PortraitId,
                //MusicProvider = musicProvider.Descriptor,
                ExternalLink = new Uri(artist.ExternalUrls["spotify"])
            };

            if (artist.Images != null && artist.Images.Any())
            {
                // TODO: More null checking?
                artistResult.PortraitUrlLarge = artist.Images[0].Url;
                artistResult.PortraitUrlMedium = artist.Images[1].Url;
                artistResult.PortraitUrlSmall = artist.Images[2].Url;                
            }

            // TODO: Figure out how to not do this...
            artistResult.MusicProvider = new MusicProviderDescriptor()
            {
                Identifier = "sp",
                Name = "Spotify"
            };

            return artistResult;

        }
    }
}