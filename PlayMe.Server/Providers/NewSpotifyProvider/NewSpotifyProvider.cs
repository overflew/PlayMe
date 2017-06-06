using PlayMe.Plumbing.Diagnostics;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Server.Providers.NewSpotifyProvider
{
    public class NewSpotifyProvider
    {
        private readonly ILogger logger;
        private readonly INewSpotifySettings spotifySettings;

        private static SpotifyWebAPI _client;
        private Token _currentToken;
        
        public NewSpotifyProvider(
            ILogger logger,
            INewSpotifySettings spotifySettings)
        {
            this.logger = logger;
            this.spotifySettings = spotifySettings;
        }

        public SpotifyWebAPI GetClient()
        {
            if (_client == null                
                || IsTokenNearExpiry()
                || (_currentToken != null 
                    && _currentToken.IsExpired()) // Safety fallback...
                )
            {
                _client = CreateClient();
            }

            return _client;
        }


        private SpotifyWebAPI CreateClient()
        {
            //Create the auth object
            var auth = new ClientCredentialsAuth()
            {
                //Your client Id
                ClientId = spotifySettings.ClientId,
                
                //Your client secret UNSECURE!!
                ClientSecret = spotifySettings.ClientSecret,
                
                //How many permissions we need?
                Scope = Scope.UserReadPrivate,
            };

            //With this token object, we now can make calls
             _currentToken = auth.DoAuth();

            var spotify = new SpotifyWebAPI()
            {
                TokenType = _currentToken.TokenType,
                AccessToken = _currentToken.AccessToken,
                UseAuth = true
            };

            return spotify;
        }

        private bool IsTokenNearExpiry()
        {
            if (_currentToken == null)
            {
                return false;
            }

            var assumedAuthTimeout = _currentToken.CreateDate.AddSeconds(_currentToken.ExpiresIn);
            
            // TODO: Will this fail @ daylight savings?

            return DateTime.Now > assumedAuthTimeout.AddMinutes(-2);
        }
    }
}
