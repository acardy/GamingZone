using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(GamingZone.Hubs.Startup))]
namespace GamingZone.Hubs
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}