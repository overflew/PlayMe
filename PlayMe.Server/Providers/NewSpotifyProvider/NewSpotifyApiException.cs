using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Server.Providers.NewSpotifyProvider
{
    public class NewSpotifyApiException : Exception
    {
        public Error SpotifyClientError
        {
            get;
            private set;
        }

        private string _message;
        public override string Message
        {
            get
            {
                return _message;
            }
        }

        public NewSpotifyApiException(
            string message,
            Error spotifyClientError)
        {
            _message = message;
            SpotifyClientError = spotifyClientError;
        }
    }
}
