using System;
using PlayMe.Common.Util;

namespace PlayMe.Server.AutoPlay.MultiAutoPlay.Config
{
    public interface IWeightedAutoPlay : IWeighted
    {
        string Name { get; }
    }
}
