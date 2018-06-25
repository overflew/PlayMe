using PlayMe.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Server.AutoPlay.Songkick
{
    public interface ISongkickConfig
    {
        string apiKey { get; }

        int RegionId { get; }

        IList<ISongKickCity> MultiSongkickConfigs { get; }

        //int HeadlinerWeight { get; }
        //int SupportActWeight { get; }

        IList<IBadGenre> BadGenres { get; }
    }

    public interface ISongKickCity : IWeighted
    {
        int Id { get; }
        string Name { get; }
    }
    
    public interface IBadGenre
    {
        string Contains { get; }
    }
}
