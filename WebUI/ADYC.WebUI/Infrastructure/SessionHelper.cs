using ADYC.WebUI.ViewModels;
using Newtonsoft.Json;
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace ADYC.WebUI.Infrastructure
{
    public class SessionHelper
    {
        public static bool ExistUserInSession()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }
        public static void DestroyUserSession()
        {
            FormsAuthentication.SignOut();
        }
        public static CustomPrincipal GetUser()
        {
            CustomPrincipal newUser = null;

            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity is IIdentity)
            {
                return (CustomPrincipal)HttpContext.Current.User;
            }

            return newUser;
        }
        public static void AddUserToSession(Token token)
        {
            var serializeModel = new CustomPrincipalSerializeModel();
            serializeModel.UserId = token.UserId;
            serializeModel.UserName = token.UserName;
            serializeModel.Role = token.Role;
            serializeModel.AccessToken = token.AccessToken;
            serializeModel.TokenType = token.TokenType;
            serializeModel.ExpiresIn = token.ExpiresIn;

            var userData = JsonConvert.SerializeObject(serializeModel);

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
            1,
            token.UserName,
            DateTime.Now,
            DateTime.Now.AddMinutes(30),
            false, //pass here true, if you want to implement remember me functionality
            userData);

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            HttpContext.Current.Response.Cookies.Add(faCookie);
        }
    }
}