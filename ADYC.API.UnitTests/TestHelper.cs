using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;

namespace ADYC.API.UnitTests
{
    public static class TestHelper
    {
        public static void SetUpControllerRequest(ApiController controller, string controllerName)
        {
            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/Courses")
            };

            controller.Configuration = new HttpConfiguration();

            controller.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", controllerName } });
        }

        public static IList<ValidationResult> Validate(object model)
        {
            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);

            Validator.TryValidateObject(model, validationContext, results, true);

            if (model is IValidatableObject)
            {
                (model as IValidatableObject).Validate(validationContext);
            }

            return results;
        }
    }
}
