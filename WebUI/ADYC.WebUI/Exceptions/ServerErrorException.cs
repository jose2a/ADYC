using ADYC.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ADYC.WebUI.Exceptions
{
    public class ServerErrorException : AdycHttpRequestException
    {
        public ServerErrorException(HttpStatusCode statusCode, string message)
            : base(statusCode, message)
        {

        }
    }
}