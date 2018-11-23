using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace ADYC.API.Exceptions
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public async override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            // Access Exception using context.Exception;  
            var exception = context.Exception;

            var controller = GetCurrentController();
            var action = GetCurrentAction();

            var logger = LogManager.GetCurrentClassLogger();

            logger.Error(exception, $"Error occured in {controller} controller {action} Action");

            var errorMessage = "An unexpected error has happend. We're working on resolving it.";

            var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError,
                new
                {
                    Message = errorMessage
                });

            response.Headers.Add("X-Error", errorMessage);
            context.Result = new ResponseMessageResult(response);
        }

        public static string GetCurrentController()
        {
            return HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
        }
        public static string GetCurrentAction()
        {
            return HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
        }
    }
}