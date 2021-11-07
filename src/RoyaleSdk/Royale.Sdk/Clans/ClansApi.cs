using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Royale.Sdk.Clans.Models;

namespace Royale.Sdk.Clans
{
    public class ClansApi : IClansApi
    {
        private readonly IMemoryCache _cache;
        private readonly JsonSerializerOptions _jsonOptions = new() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
        private readonly string _token;

        public ClansApi(IConfiguration config, IMemoryCache cache)
        {
            _token = config["ApiToken"];
            if (string.IsNullOrWhiteSpace(_token))
            {
                throw new RoyaleSdkException("Api Token not found. Please define an 'ApiToken' property in the appsettings.json.");
            }

            _cache = cache;
        }

        public async Task<Clan> GetClan(string clanTag)
        {
            if (clanTag == null) throw new ArgumentNullException(nameof(clanTag));

            if (_cache.TryGetValue(clanTag, out var cached))
            {
                return JsonSerializer.Deserialize<Clan>((string)cached, _jsonOptions);
            }

            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.clashroyale.com/v1/clans/")
            };

            var decodedClanTag = clanTag.StartsWith("#") ? UrlEncoder.Default.Encode(clanTag) : $"%23{clanTag}";

            var request = new HttpRequestMessage(HttpMethod.Get, decodedClanTag);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new RoyaleSdkNetworkException(content, response.StatusCode);
            }

            _cache.Set(clanTag, content);
            return JsonSerializer.Deserialize<Clan>(content, _jsonOptions);
        }
    }
}