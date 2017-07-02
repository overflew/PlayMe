using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Server.AutoPlay.UseMachineLearningApi
{
    public class UseMachineLearningResolver : NinjectModule
    {
        public override void Load()
        {
            Bind<IStandAloneAutoPlay>().To<UseMachineLearningApiAutoplay>();
            Bind<IMachineLearningApiService>().To<MachineLearningApiService>();
        }
    }
}
