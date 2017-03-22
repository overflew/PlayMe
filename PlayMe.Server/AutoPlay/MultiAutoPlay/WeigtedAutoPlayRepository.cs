using Nerdle.AutoConfig;
using PlayMe.Server.AutoPlay.MultiAutoPlay.Config;
using System.Collections.Generic;
using System.Linq;

namespace PlayMe.Server.AutoPlay.MultiAutoPlay
{
    public class WeigtedAutoPlayRepository : IWeigtedAutoPlayRepository
    {
        public IList<IWeightedAutoPlay> GetAllAutoPlays()
        {
            var config = AutoConfig.Map<IWeightedAutoPlayConfig>();

            return config.WeightedAutoPlays.ToList();
        }
    }
}
