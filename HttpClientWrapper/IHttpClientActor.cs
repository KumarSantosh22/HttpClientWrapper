using HttpClientWrapper.Helpers;

namespace HttpClientWrapper
{
    public interface IHttpClientActor
    {
        Task<TResponse> GetAsync<TResponse>(string endpoint);
        Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest body, string contentType = BagOfWords.ApplicationJson);
        Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest body, string contentType = BagOfWords.ApplicationJson);
        Task<TResponse> DeleteAsync<TResponse>(string endpoint);
        void Dispose();
        void SetBaseUri(string baseUri);
        void SetToken(string token, string scheme = "Bearer");
        void SetHeader(string key, string value);
        void SetHeaders(Dictionary<string, string> headers);
        bool RemoveHeader(string key);
        void RemoveHeaders(List<string> keys);
    }
}