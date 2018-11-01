using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace ADYC.WebUI.Repositories
{
    public class LoginRepository : IDisposable
    {
        private bool disposed = false;

        private string addressPreffix = "/Token";
        private static string jsonMediaType = "application/json";

        private HttpClient httpClient = MakeHttpClient();

        private static HttpClient MakeHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:13303/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(jsonMediaType));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("ADYC_HttpClient", "1.0")));
            return client;
        }

        public async Task<Token> Login(LoginFormViewModel model)
        {
            var form = new Dictionary<string, string>
               {
                   {"grant_type", "password"},
                   {"username", model.UserName},
                   {"password", model.Password},
               };

            var tokenResponse = await httpClient.PostAsync(addressPreffix, new FormUrlEncodedContent(form));
            
            var token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;

            if (string.IsNullOrEmpty(token.Error))
            {
                return token;
            }
            else
            {
                throw new AdycHttpRequestException(System.Net.HttpStatusCode.BadRequest, token.Error);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                if (httpClient != null)
                {
                    var mc = httpClient;
                    httpClient = null;
                    mc.Dispose();
                }
                disposed = true;
            }
        }
    }
}