using ADYC.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ADYC.WebUI.Exceptions
{
    public class BadRequestException : AdycHttpRequestException
    {
        public BadRequestException(HttpStatusCode statusCode, string message)
            : base(statusCode, message)
        {

        }

        public BadRequestException(HttpStatusCode statusCode, string message, IList<string> errors)
            : base(statusCode, message, errors)
        {

        }
    }
}