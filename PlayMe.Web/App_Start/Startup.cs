using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(PlayMe.Web.App_Start.Startup))]
namespace PlayMe.Web.App_Start
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Enable CORS for all external apps. Allows both SignalR & WebAPI calls. Yay!
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();      
        }
    }
}