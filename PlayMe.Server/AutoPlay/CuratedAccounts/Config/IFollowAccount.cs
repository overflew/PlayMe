using PlayMe.Common.Util;

namespace PlayMe.Server.AutoPlay.CuratedAccounts.Config
{
    public interface IFollowAccount : IWeighted
    {
        
        string User { get; }

        string DisplayName { get; }
    }
}
