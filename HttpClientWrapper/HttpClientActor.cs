using HttpClientWrapper.Helpers;
using System.Net.Http.Headers;
using System.Text.Json;

namespace HttpClientWrapper
{
    public class HttpClientActor : IHttpClientActor, IDisposable
    {
        internal HttpClient client;
        private JsonSerializerOptions _options;

        public HttpClientActor()
        {
            client = new HttpClient();
            ConfigureSerializer();
        }

        public HttpClientActor(string uri)
        {
            client = new HttpClient();
            SetBaseUri(uri);
            ConfigureSerializer();
        }

        private void ConfigureSerializer()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public void SetBaseUri(string baseUri)
        {
            client.BaseAddress = new Uri(baseUri);
        }

        public async Task<TResponse> GetAsync<TResponse>(string endpoint)
        {
            HttpResponseMessage response = await client.GetAsync(endpoint);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(responseContent);
            }
            TResponse data = JsonSerializer.Deserialize<TResponse>(responseContent, _options);
            return data;
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest body, string contentType = BagOfWords.ApplicationJson)
        {
            string json = JsonSerializer.Serialize(body, _options);
            StringContent httpContent = new(json, System.Text.Encoding.UTF8, contentType);

            HttpResponseMessage response = await client.PostAsync(endpoint, httpContent);
            string content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            TResponse data = JsonSerializer.Deserialize<TResponse>(content, _options);
            return data;
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest body, string contentType = BagOfWords.ApplicationJson)
        {
            string json = JsonSerializer.Serialize(body, _options);
            StringContent httpContent = new(json, System.Text.Encoding.UTF8, contentType);

            HttpResponseMessage response = await client.PutAsync(endpoint, httpContent);
            string content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            TResponse data = JsonSerializer.Deserialize<TResponse>(content, _options);
            return data;
        }

        public async Task<TResponse> DeleteAsync<TResponse>(string endpoint)
        {
            HttpResponseMessage response = await client.DeleteAsync(endpoint);
            string content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            TResponse data = JsonSerializer.Deserialize<TResponse>(content, _options);
            return data;
        }

        public void SetToken(string token, string scheme = BagOfWords.Bearer)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);
        }

        public void SetHeader(string key, string value)
        {
            client.DefaultRequestHeaders.Add(key, value);
        }

        public void SetHeaders(Dictionary<string, string> headers)
        {
            foreach (KeyValuePair<string, string> keyValuePair in headers)
            {
                client.DefaultRequestHeaders.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public bool RemoveHeader(string key)
        {
            return client.DefaultRequestHeaders.Remove(key);
        }

        public void RemoveHeaders(List<string> keys)
        {
            foreach (string key in keys)
            {
                _ = client.DefaultRequestHeaders.Remove(key);
            }
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}