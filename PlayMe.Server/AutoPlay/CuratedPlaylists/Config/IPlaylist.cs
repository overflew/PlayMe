using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayMe.Common.Util;

namespace PlayMe.Server.AutoPlay.CuratedPlaylists.Config
{
    public interface IPlaylist : IWeighted
    {
        string PlaylistId { get; }

        string User { get; }

        string PlaylistName { get; }
    }
}
