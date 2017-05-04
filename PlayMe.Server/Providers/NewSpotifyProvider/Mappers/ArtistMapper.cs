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
                PortraitUrlLarge = artist.Images[0].Url,
                PortraitUrlMedium = artist.Images[1].Url,
                PortraitUrlSmall = artist.Images[2].Url,
                //PortraitId = artist.PortraitId,
                //MusicProvider = musicProvider.Descriptor,
                ExternalLink = new Uri(artist.ExternalUrls["spotify"])
            };

            return artistResult;

        }
    }
}