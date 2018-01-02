using Game.Web.App_Start;
using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(Startup))]
namespace Game.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}