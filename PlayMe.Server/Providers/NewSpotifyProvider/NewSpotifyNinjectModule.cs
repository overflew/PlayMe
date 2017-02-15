using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayMe.Server.Providers.NewSpotifyProvider
{
    public class NewSpotifyNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<INewSpotifySettings>().To<NewSpotifySettings>();
        }
    }
}
