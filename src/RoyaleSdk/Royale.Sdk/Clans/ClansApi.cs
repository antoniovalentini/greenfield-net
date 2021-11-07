using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Royale.Sdk.Clans.Models;

namespace Royale.Sdk.Clans
{
    public class ClansApi : IClansApi
    {
        private const string CachePrefix = "CLAN_";
        private const string ApiPath = "clans/";
        private readonly IMemoryCache _cache;
        private readonly IApiClient _apiClient;
        private readonly JsonSerializerOptions _jsonOptions = SdkSerializerOptions.JsonOptions;

        public ClansApi(IMemoryCache cache, IApiClient apiClient)
        {
            _cache = cache;
            _apiClient = apiClient;
        }

        public async Task<Clan> GetClan(string clanTag)
        {
            if (clanTag == null) throw new ArgumentNullException(nameof(clanTag));

            if (_cache.TryGetValue(CachePrefix + clanTag, out var cached))
            {
                return JsonSerializer.Deserialize<Clan>((string)cached, _jsonOptions);
            }

            var decodedClanTag = clanTag.StartsWith("#") ? UrlEncoder.Default.Encode(clanTag) : $"%23{clanTag}";

            var (value, raw) = await _apiClient.GetAsync<Clan>(ApiPath + decodedClanTag);

            _cache.Set(CachePrefix + clanTag, raw);

            return value;
        }
    }
}
