﻿using Newtonsoft.Json.Linq;
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
        private bool disposed = false;

        private HttpClient httpClient;

        protected readonly string serviceBaseAddress;
        private readonly string jsonMediaType = "application/json";

        public GenericRestfulCrudHttpClient(string serviceBaseAddress)
        {
            this.serviceBaseAddress = serviceBaseAddress;
            httpClient = MakeHttpClient(serviceBaseAddress);
        }

        protected virtual HttpClient MakeHttpClient(string serviceBaseAddress)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(serviceBaseAddress);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(jsonMediaType));
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("ADYC_HttpClient", "1.0")));
            return httpClient;
        }

        public async Task<IEnumerable<T>> GetManyAsync(string addressSuffix)
        {
            var responseMessage = await httpClient.GetAsync(addressSuffix);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadAsAsync<IEnumerable<T>>();
        }

        public async Task<T> GetAsync(string addressSuffix)
        {
            var responseMessage = await httpClient.GetAsync(addressSuffix);
            await VerifyResponseStatus(responseMessage);

            return await responseMessage.Content.ReadAsAsync<T>();
        }

        public async Task<HttpStatusCode> GetAsyncWithStatusCode(string addressSuffix)
        {
            var responseMessage = await httpClient.GetAsync(addressSuffix);
            await VerifyResponseStatus(responseMessage);

            return responseMessage.StatusCode;
        }

        public async Task<T> PostAsync(string addressSuffix, T model)
        {
            var responseMessage = await httpClient.PostAsJsonAsync(addressSuffix, model);
            await VerifyResponseStatus(responseMessage);

            return await responseMessage.Content.ReadAsAsync<T>();
        }

        public async Task<HttpStatusCode> PutAsync(string addressSuffix, T model)
        {
            var responseMessage = await httpClient.PutAsJsonAsync(addressSuffix, model);
            await VerifyResponseStatus(responseMessage);

            return responseMessage.StatusCode;
        }

        public async Task<HttpStatusCode> DeleteAsync(string addressSuffix)
        {
            var responseMessage = await httpClient.DeleteAsync(addressSuffix);
            await VerifyResponseStatus(responseMessage);

            return responseMessage.StatusCode;
        }

        private List<string> GetModelStateErrors(string content)
        {
            List<string> errors = new List<string>();

            var contentObject = JObject.Parse(content);
            var modelState = contentObject.GetValue("ModelState").ToObject<JObject>();

            foreach (var property in modelState)
            {
                var arr = JArray.Parse(property.Value.ToString());

                foreach (var item in arr)
                {
                    errors.Add(item.Value<string>());
                }
            }

            return errors;
        }

        private async Task VerifyResponseStatus(HttpResponseMessage responseMessage)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                var content = await responseMessage.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(content))
                {
                    var errors = GetModelStateErrors(content);

                    throw new AdycHttpRequestException(responseMessage.StatusCode,
                        responseMessage.ReasonPhrase,
                        errors);
                }                
            }
        }

        #region IDisposable Members

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
                    var hc = httpClient;
                    httpClient = null;
                    hc.Dispose();
                }
                disposed = true;
            }
        }

        #endregion IDisposable Members
    }
}