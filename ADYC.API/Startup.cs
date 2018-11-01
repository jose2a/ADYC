using Microsoft.Owin;
using Owin;

//[assembly: OwinStartupAttribute("ResourcesConfig", typeof(ADYC.API.StartupApi))]
[assembly: OwinStartup("ResourcesConfig", typeof(ADYC.API.StartupApi))]
namespace ADYC.API
{
    public partial class StartupApi
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);
        }
    }
}
