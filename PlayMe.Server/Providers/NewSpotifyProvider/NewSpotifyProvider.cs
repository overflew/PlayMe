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

            if (_client == null)
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
            Token token = auth.DoAuth();
            var spotify = new SpotifyWebAPI()
            {
                TokenType = token.TokenType,
                AccessToken = token.AccessToken,
                UseAuth = true
            };

            return spotify;
        }
    }
}
