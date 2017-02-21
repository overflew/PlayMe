using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayMe.Common.Util;

namespace PlayMe.Server.AutoPlay.CuratedPlaylists
{
    public class PlaylistModel : IWeighted
    {
        public PlaylistModel()
        {
            // Default value
            Weight = 1;
        }

        public string PlaylistId { get; set; }

        public string User { get; set; }

        public string PlaylistName { get; set; }

        public int Weight { get; set; }
    }
}
