using Microsoft.Owin.Security.OAuth;
using Owin;

namespace ADYC.API
{
    public partial class StartupApi
    {
        private void ConfigureOAuth(IAppBuilder app)
        {
            //Token Consumption
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
            {
            });
        }
    }
}
