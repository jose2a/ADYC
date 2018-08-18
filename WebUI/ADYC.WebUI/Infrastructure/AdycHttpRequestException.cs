using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace ADYC.WebUI.Infrastructure
{
    public class AdycHttpRequestException : HttpRequestException
    {
        public HttpStatusCode StatusCode { get; private set; }
        public IList<string> Errors { get; private set; }

        public AdycHttpRequestException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public AdycHttpRequestException(HttpStatusCode statusCode, string message, IList<string> errors)
            : this (statusCode, message)
        {
            Errors = errors;
        }

        public void AddError(string error)
        {
            Errors.Add(error);
        }

    }
}