using PlayMe.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Server.Providers.SpotifyProvider
{
    public static class SpotifyConsts
    {
        public static MusicProviderDescriptor SpotifyMusicProviderDescriptor
        {
            get
            {
                return new MusicProviderDescriptor()
                {
                    Identifier = "sp",
                    Name = "Spotify"
                };
            }
        }
    }
}
