using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Server.Providers.NewSpotifyProvider
{
    public interface INewSpotifySettings
    {
        string ClientId { get; }
        string ClientSecret { get; }
    }

    public class NewSpotifySettings : INewSpotifySettings
    {

        public string ClientId
        {
            get
            {
                return ConfigurationManager.AppSettings["Spotify.ClientId"];
            }
        }

        public string ClientSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["Spotify.ClientSecret"];
            }
        }
    }
}
