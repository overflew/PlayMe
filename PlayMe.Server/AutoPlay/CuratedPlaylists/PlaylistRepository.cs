using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayMe.Common.Util;

namespace PlayMe.Server.AutoPlay.CuratedPlaylists
{
    interface IPlaylistRepository
    {
        PlaylistModel GetRandomPlaylist();

        IList<PlaylistModel> GetAllPlaylists();
    }

    public class PlaylistRepository : IPlaylistRepository
    {
        public PlaylistModel GetRandomPlaylist()
        {
            var playlists = GetAllPlaylists();

            return WeightingUtil.Choose(playlists);
        }

        public IList<PlaylistModel> GetAllPlaylists()
        {
            // TODO: Move this out to a .config file.
            return new List<PlaylistModel>()
            {
                new PlaylistModel() {
                    PlaylistName = "BBC 6 Music recommends",
                    User = "bbc_playlister",
                    PlaylistId = "6ToRtiBeKUf0py8gZO6gQj",
                    Weight = 10
                },
                new PlaylistModel()
                {
                    PlaylistName = "Hottest record in the world",
                    User = "bbc_playlister",
                    PlaylistId = "1Snu2f6yIWWufuqYkYaYKR",
                    Weight = 10
                },
                new PlaylistModel()
                {
                    PlaylistName = "Double J Hitlist",
                    User = "doublejradio",
                    PlaylistId = "3eVaP90RyWrOKu6Gejw5Eg",
                    Weight = 10
                },
                new PlaylistModel()
                {
                    PlaylistName = "Josh Homme's Alligator Hour",
                    User = "1262019033",
                    PlaylistId = "4m4bGVPdfssEkn7u3CVQFd",
                    Weight = 10
                },
                new PlaylistModel()
                {
                    PlaylistName = "The Definitive Later... with Jools",
                    User = "ajpegg",
                    PlaylistId = "1mWt9unHFcjyAT1SVI985d",
                    Weight = 10
                }
            };
        }
    }
}
