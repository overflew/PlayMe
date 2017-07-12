using System.Collections.Generic;

namespace PlayMe.Server.AutoPlay.CuratedAccounts.Config
{
    public interface IFollowAccountConfig
    {
        IEnumerable<IFollowAccount> FollowAccounts { get; }
    }
}
