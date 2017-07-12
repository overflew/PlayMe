using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Server.AutoPlay.CuratedPlaylists.Config
{
    public interface IPlaylistConfig
    {
        IEnumerable<IPlaylist> Playlists { get; }
    }
}
