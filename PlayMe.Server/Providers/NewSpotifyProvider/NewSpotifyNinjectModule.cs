using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayMe.Server.Providers.NewSpotifyProvider.Mappers;

namespace PlayMe.Server.Providers.NewSpotifyProvider
{
    public class NewSpotifyNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<INewSpotifySettings>().To<NewSpotifySettings>();

            Bind<ITrackMapper>().To<TrackMapper>();
            Bind<IArtistMapper>().To<ArtistMapper>();
            Bind<IAlbumMapper>().To<AlbumMapper>();

            Bind<ISpotifySearchService>().To<NewSpotifySearchService>();
        }
    }
}
