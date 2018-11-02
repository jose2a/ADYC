using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace ADYC.WebUI.Infrastructure
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            return Role.Equals(role);
            //if (roles.Any(r => role.Contains(r)))
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        public CustomPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }

        public Guid UserId { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        public string Role { get; set; }
    }
}