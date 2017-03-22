using PlayMe.Server.AutoPlay.MultiAutoPlay.Config;
using System.Collections.Generic;

namespace PlayMe.Server.AutoPlay.MultiAutoPlay
{
    public interface IWeigtedAutoPlayRepository
    {
        IList<IWeightedAutoPlay> GetAllAutoPlays();
    }
}
