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
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadAsAsync<T>();
        }

        public async Task<T> PostAsync(string addressSuffix, T model)
        {

            var responseMessage = await httpClient.PostAsJsonAsync(addressSuffix, model);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var message = await responseMessage.Content.ReadAsStringAsync();
                throw new HttpRequestException(message);
            }

            return await responseMessage.Content.ReadAsAsync<T>();
        }

        public async Task<HttpStatusCode> PutAsync(string addressSuffix, T model)
        {
            var responseMessage = await httpClient.PutAsJsonAsync(addressSuffix, model);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var message = await responseMessage.Content.ReadAsStringAsync();
                throw new HttpRequestException(message);
            }

            return responseMessage.StatusCode;
        }

        public async Task<HttpStatusCode> DeleteAsync(string addressSuffix)
        {
            var responseMessage = await httpClient.DeleteAsync(addressSuffix);

            if (!responseMessage.IsSuccessStatusCode)
            {
                var message = await responseMessage.Content.ReadAsStringAsync();
                throw new HttpRequestException(message);
            }

            return responseMessage.StatusCode;
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