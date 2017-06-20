using System.Collections.Generic;

namespace PlayMe.Server.AutoPlay.MultiAutoPlay.Config
{
    public interface IWeightedAutoPlayConfig
    {
        IEnumerable<IWeightedAutoPlay> WeightedAutoPlays { get; }
    }
}
