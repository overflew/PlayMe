using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Server.AutoPlay.Recommendations
{
    public interface IRecommendationsConfig
    {
        int UserTracksSeeds { get; }

        int AutoplayPopularSeeds { get; }

        int LikeToVetoSeedAcceptanceRatio { get; }

        int PointsBase { get; }

        int PointsForAnyLikes { get; }

        int PointsForAnyVetoes { get; }
    }
}
