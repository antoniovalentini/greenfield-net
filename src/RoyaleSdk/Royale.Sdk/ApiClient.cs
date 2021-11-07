using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Royale.Sdk
{
    public interface IApiClient
    {
        Task<ApiResponse<T>> GetAsync<T>(string requestUri);
    }

    public class ApiClient : IApiClient
    {
        private readonly JsonSerializerOptions _jsonOptions = SdkSerializerOptions.JsonOptions;
        private readonly HttpClient _httpClient;
        private readonly string _token;

        public ApiClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _token = config["ApiToken"];
            if (string.IsNullOrWhiteSpace(_token))
            {
                throw new RoyaleSdkException("Api Token not found. Please define an 'ApiToken' property in the appsettings.json.");
            }
        }

        public async Task<ApiResponse<T>> GetAsync<T>(string requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new RoyaleSdkNetworkException(content, response.StatusCode);
            }

            var value = JsonSerializer.Deserialize<T>(content, _jsonOptions);

            return new ApiResponse<T>(value, content);
        }
    }
}
