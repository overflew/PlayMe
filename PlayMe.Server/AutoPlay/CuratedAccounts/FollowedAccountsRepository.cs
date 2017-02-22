using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nerdle.AutoConfig;
using PlayMe.Common.Util;
using PlayMe.Server.AutoPlay.CuratedAccounts.Config;
using PlayMe.Server.AutoPlay.CuratedPlaylists.Config;

namespace PlayMe.Server.AutoPlay.CuratedAccounts
{
    public class FollowedAccountsRepository
    {
        public IFollowAccount GetRandomAccount()
        {
            var playlists = GetAllAccounts();

            return WeightingUtil.ChooseWeightedRandom(playlists);
        }

        public IList<IFollowAccount> GetAllAccounts()
        {
            var config = AutoConfig.Map<IFollowAccountConfig>();

            return config.FollowAccounts.ToList();
        }
    }
}
