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

        private SpotifyWebAPI _client;
        private Token _currentToken;
        private DateTime _currentAssumedAuthTimeout;

        public NewSpotifyProvider(
            ILogger logger,
            INewSpotifySettings spotifySettings)
        {
            this.logger = logger;
            this.spotifySettings = spotifySettings;
        }

        public SpotifyWebAPI GetClient()
        {
            // TODO: The auth token will eventually timeout. How can we test it, to decide when to re-auth?
            // (there is a token.ExpiresIn property below. May it may crap out for other reasons?)

            if (_client == null
                || _currentToken.IsExpired() // TODO: Does this library calculate this field? (Will it tick over to false?)
                || IsTokenNearExpiry())
            {
                _client = CreateClient();
            }

            return _client;
        }


        public SpotifyWebAPI CreateClient()
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

            _currentAssumedAuthTimeout = _currentToken.CreateDate.AddSeconds(_currentToken.ExpiresIn);

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
            if (_currentAssumedAuthTimeout == null)
            {
                return false;
            }

            // TODO: Will this fail @ daylight savings?

            return DateTime.Now > _currentAssumedAuthTimeout.AddMinutes(-2);
        }
    }
}
