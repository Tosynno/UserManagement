using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.IntegrationTest.Services
{
    public class HttpClientService
    {
        private readonly HttpClient _httpClient;

        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private void AddHeaders(Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }

        public async Task<T> GetAsync<T>(string uri, Dictionary<string, string> headers = null)
        {
            AddHeaders(headers);

            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T> PostAsync<T>(string uri, object data, Dictionary<string, string> headers = null)
        {
            AddHeaders(headers);

            var response = await _httpClient.PostAsJsonAsync(uri, data);
            //response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<HttpResponseMessage> PostAsync(string uri, object data, Dictionary<string, string> headers = null)
        {
            AddHeaders(headers);

            var response = await _httpClient.PostAsJsonAsync(uri, data);
            return response;
        }

        public async Task<T> PutAsync<T>(string uri, object data, Dictionary<string, string> headers = null)
        {
            AddHeaders(headers);

            var response = await _httpClient.PutAsJsonAsync(uri, data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task DeleteAsync(string uri, Dictionary<string, string> headers = null)
        {
            AddHeaders(headers);

            var response = await _httpClient.DeleteAsync(uri);
            //response.EnsureSuccessStatusCode();
        }
    }
}
