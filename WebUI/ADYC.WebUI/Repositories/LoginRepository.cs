using ADYC.WebUI.Exceptions;
using ADYC.WebUI.ViewModels;
using RestSharp;
using System;
using System.Collections.Generic;

namespace ADYC.WebUI.Repositories
{
    public class LoginRepository : IDisposable
    {
        private bool _disposed = false;

        private string _addressPreffix = "/Token";
        private const string jsonMediaType = "application/json";

        private RestClient client;

        private RestClient MakeRestClient()
        {
            if (client == null)
            {
                client = new RestClient("http://localhost:13303");
            }

            return client;
        }

        public Token Login(LoginFormViewModel model)
        {
            client = MakeRestClient();

            var form = new Dictionary<string, string>
               {
                   {"grant_type", "password"},
                   {"username", model.UserName},
                   {"password", model.Password},
               };

            var request = new RestRequest(_addressPreffix, Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddParameter("grant_type", "password")
                .AddParameter("username", model.UserName)
                .AddParameter("password", model.Password);

            IRestResponse<Token> response = client.Execute<Token>(request);

            var token = response.Data;

            if (!string.IsNullOrEmpty(token.Error))
            {                
                throw new BadRequestException(System.Net.HttpStatusCode.BadRequest, token.Error);
            }

            return token;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (client != null)
                {
                    client = null;
                }
                _disposed = true;
            }
        }
    }
}