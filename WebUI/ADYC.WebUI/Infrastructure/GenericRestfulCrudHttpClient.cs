using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ADYC.WebUI.Infrastructure
{
    public class GenericRestfulCrudHttpClient<T> : IDisposable where T : class
    {
        private HttpClient _httpClient;
        private bool _disposed = false;

        private string _serviceBaseAddress;
        private bool _isAccessTokenRequired;

        private const string JsonMediaType = "application/json";

        public GenericRestfulCrudHttpClient(string serviceBaseAddress, bool isAccessTokenRequired)
        {
            _serviceBaseAddress = serviceBaseAddress;
            _isAccessTokenRequired = isAccessTokenRequired;
        }

        protected virtual HttpClient MakeHttpClient()
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
                _httpClient.BaseAddress = new Uri(_serviceBaseAddress);
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(JsonMediaType));
                _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("ADYC_HttpClient", "1.0")));

                if (_isAccessTokenRequired)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", SessionHelper.User.AccessToken);
                }
            }

            return _httpClient;
        }

        public async Task<IEnumerable<T>> GetManyAsync(string addressSuffix)
        {
            _httpClient = MakeHttpClient();

            var responseMessage = await _httpClient.GetAsync(addressSuffix);

            await VerifyResponseStatus(responseMessage);

            //responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadAsAsync<IEnumerable<T>>();
        }

        public async Task<T> GetAsync(string addressSuffix)
        {
            _httpClient = MakeHttpClient();

            var responseMessage = await _httpClient.GetAsync(addressSuffix);

            await VerifyResponseStatus(responseMessage);

            return await responseMessage.Content.ReadAsAsync<T>();
        }

        public async Task<HttpStatusCode> GetAsyncWithStatusCode(string addressSuffix)
        {
            _httpClient = MakeHttpClient();

            var responseMessage = await _httpClient.GetAsync(addressSuffix);

            //if (VerifyNotFound(responseMessage))
            //{
            //    return HttpStatusCode.NotFound;
            //}

            //await VerifyResponseStatus(responseMessage);

            return responseMessage.StatusCode;
        }

        public async Task<T> PostAsync(string addressSuffix, T model)
        {
            _httpClient = MakeHttpClient();

            var responseMessage = await _httpClient.PostAsJsonAsync(addressSuffix, model);

            await VerifyResponseStatus(responseMessage);

            return await responseMessage.Content.ReadAsAsync<T>();
        }

        public async Task<HttpStatusCode> PutAsync(string addressSuffix, T model)
        {
            _httpClient = MakeHttpClient();

            var responseMessage = await _httpClient.PutAsJsonAsync(addressSuffix, model);

            //if (VerifyNotFound(responseMessage))
            //{
            //    return HttpStatusCode.NotFound;
            //}

            await VerifyResponseStatus(responseMessage);

            return responseMessage.StatusCode;
        }

        public async Task<HttpStatusCode> DeleteAsync(string addressSuffix)
        {
            _httpClient = MakeHttpClient();

            var responseMessage = await _httpClient.DeleteAsync(addressSuffix);

            //if (VerifyNotFound(responseMessage))
            //{
            //    return HttpStatusCode.NotFound;
            //}

            await VerifyResponseStatus(responseMessage);

            return responseMessage.StatusCode;
        }

        private List<string> GetModelStateErrors(string content)
        {
            List<string> errors = new List<string>();

            var contentObject = JObject.Parse(content);

            if (contentObject.ContainsKey("ModelState"))
            {
                var modelState = contentObject.GetValue("ModelState").ToObject<JObject>();

                if (modelState != null)
                {
                    foreach (var property in modelState)
                    {
                        var arr = JArray.Parse(property.Value.ToString());

                        foreach (var item in arr)
                        {
                            errors.Add(item.Value<string>());
                        }
                    }
                } 
            }

            return errors;
        }

        private async Task VerifyResponseStatus(HttpResponseMessage responseMessage)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                //if (responseMessage.StatusCode == HttpStatusCode.NotFound)
                //{
                //    throw new AdycHttpRequestException(responseMessage.StatusCode,
                //        responseMessage.ReasonPhrase);
                //}

                //if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                //{
                //    throw new AdycHttpRequestException(responseMessage.StatusCode,
                //        responseMessage.ReasonPhrase);
                //}

                var content = await responseMessage.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(content))
                {
                    var errors = GetModelStateErrors(content);

                    throw new AdycHttpRequestException(responseMessage.StatusCode,
                        responseMessage.ReasonPhrase,
                        errors);
                }

                throw new AdycHttpRequestException(responseMessage.StatusCode,
                    responseMessage.ReasonPhrase);
            }
        }

        private bool VerifyNotFound(HttpResponseMessage responseMessage)
        {
            return responseMessage.StatusCode == HttpStatusCode.NotFound;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_httpClient != null)
                {
                    var hc = _httpClient;
                    _httpClient = null;
                    hc.Dispose();
                }
                _disposed = true;
            }
        }

        #endregion IDisposable Members
    }
}