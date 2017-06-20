using System.Collections.Generic;
using System.Linq;

namespace PlayMe.Server.AutoPlay.MultiAutoPlay
{
    public class AutoPlayResolver
    {
        private IList<IStandAloneAutoPlay> autoPlays;
        public AutoPlayResolver(IList<IStandAloneAutoPlay> autoPlays)
        {
            this.autoPlays = autoPlays;
        }

        public IAutoPlay GetAutoPlayInstance(string name)
        {
            return autoPlays.Where(x => x.GetType().Name.Equals(name, System.StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        }
    }
}
