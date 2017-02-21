using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nerdle.AutoConfig;
using PlayMe.Common.Util;
using PlayMe.Server.AutoPlay.CuratedPlaylists.Config;

namespace PlayMe.Server.AutoPlay.CuratedPlaylists
{
    interface IPlaylistRepository
    {
        IPlaylist GetRandomPlaylist();

        IList<IPlaylist> GetAllPlaylists();
    }

    public class PlaylistRepository : IPlaylistRepository
    {
        public IPlaylist GetRandomPlaylist()
        {
            var playlists = GetAllPlaylists();

            return WeightingUtil.ChooseWeightedRandom(playlists);
        }

        public IList<IPlaylist> GetAllPlaylists()
        {
            var config = AutoConfig.Map<IPlaylistConfig>();

            return config.Playlists.ToList();
        }
    }
}
