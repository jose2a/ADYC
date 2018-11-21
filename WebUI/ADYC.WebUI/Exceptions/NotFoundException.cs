using ADYC.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace ADYC.WebUI.Exceptions
{
    public class NotFoundException : AdycHttpRequestException
    {
        public NotFoundException(HttpStatusCode statusCode, string message)
            : base(statusCode, message)
        {
                
        }
    }
}