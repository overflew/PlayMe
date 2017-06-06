using System;
using System.Collections.Generic;
using PlayMe.Common.Model;
using PlayMe.Server.AutoPlay.MultiAutoPlay.Config;
using PlayMe.Common.Util;
using PlayMe.Server.AutoPlay.MultiAutoPlay;

namespace PlayMe.Server.AutoPlay.MultiAutoplay
{
    public class MultiAutoPlay : IAutoPlay
    {
        IList<IWeightedAutoPlay> autoPlayRepository;
        AutoPlayResolver autoPlayResolver;

        public MultiAutoPlay(IWeightedAutoPlayRepository autoPlayRepository, AutoPlayResolver autoPlayResolver)
        {
            this.autoPlayRepository = autoPlayRepository.GetAllAutoPlays();

            if (this.autoPlayRepository.Count == 0)
            {
                throw new Exception("No autoPlay instances loaded by WeigtedAutoPlayProvider.");
            }

            this.autoPlayResolver = autoPlayResolver;
        }

        public QueuedTrack FindTrack()
        {
            // 1) pick random AutoPlay
            var autoPlay = WeightingUtil.ChooseWeightedRandom(autoPlayRepository);

            // 2) get instance of selected AutoPlay
            var instance = autoPlayResolver.GetAutoPlayInstance(autoPlay.Name);

            // 3) queue new track
            return instance.FindTrack();
        }
    }
}
