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

        //int HeadlinerWeight { get; }
        //int SupportActWeight { get; }
    }
}
