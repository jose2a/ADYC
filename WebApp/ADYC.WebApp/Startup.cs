using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ADYC.WebApp.Startup))]
namespace ADYC.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
