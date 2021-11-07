using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Royale.Sdk.Players.Models;

namespace Royale.Sdk.Players
{
    public class PlayesApi : IPlayersApi
    {
        private const string CachePrefix = "PLAYER_";
        private readonly IMemoryCache _cache;
        private readonly JsonSerializerOptions _jsonOptions = new() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
        private readonly string _token;

        public PlayesApi(IConfiguration config, IMemoryCache cache)
        {
            _token = config["ApiToken"];
            if (string.IsNullOrWhiteSpace(_token))
            {
                throw new RoyaleSdkException("Api Token not found. Please define an 'ApiToken' property in the appsettings.json.");
            }

            _cache = cache;
        }

        public async Task<Player> GetPlayer(string playerTag)
        {
            if (playerTag == null) throw new ArgumentNullException(nameof(playerTag));

            if (_cache.TryGetValue(CachePrefix + playerTag, out var cached))
            {
                return JsonSerializer.Deserialize<Player>((string)cached, _jsonOptions);
            }

            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.clashroyale.com/v1/players/")
            };

            var decodedPlayerTag = playerTag.StartsWith("#") ? UrlEncoder.Default.Encode(playerTag) : $"%23{playerTag}";

            var request = new HttpRequestMessage(HttpMethod.Get, decodedPlayerTag);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new RoyaleSdkNetworkException(content, response.StatusCode);
            }

            _cache.Set(CachePrefix + playerTag, content);
            return JsonSerializer.Deserialize<Player>(content, _jsonOptions);
        }
    }
}
