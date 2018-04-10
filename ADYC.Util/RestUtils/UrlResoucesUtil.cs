using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Util.RestUtils
{
    public static class UrlResoucesUtil
    {
        public static string GetBaseUrl(HttpRequestMessage request, string controllerName)
        {
            var reqUri = request.RequestUri;

            return reqUri.Scheme + "://" + reqUri.Authority + "/api/" + controllerName + "/";
        }
    }
}
