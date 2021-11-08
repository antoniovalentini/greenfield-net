using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Royale.Sdk.Players.Models;

namespace Royale.Sdk.Players
{
    public class PlayersApi : IPlayersApi
    {
        private const string CachePrefix = "PLAYER_";
        private const string ApiPath = "players/";
        private readonly IMemoryCache _cache;
        private readonly IApiClient _apiClient;
        private readonly JsonSerializerOptions _jsonOptions = SdkSerializerOptions.JsonOptions;

        public PlayersApi(IMemoryCache cache, IApiClient apiClient)
        {
            _cache = cache;
            _apiClient = apiClient;
        }

        public async Task<Player> GetPlayer(string playerTag)
        {
            if (playerTag == null) throw new ArgumentNullException(nameof(playerTag));

            if (_cache.TryGetValue(CachePrefix + playerTag, out var cached))
            {
                return JsonSerializer.Deserialize<Player>((string)cached, _jsonOptions);
            }

            var decodedPlayerTag = playerTag.StartsWith("#")
                ? UrlEncoder.Default.Encode(playerTag)
                : $"%23{playerTag}";

            var (player, raw) = await _apiClient.GetAsync<Player>(ApiPath + decodedPlayerTag);

            _cache.Set(CachePrefix + playerTag, raw);

            return player;
        }

        public async Task<GetUpcomingChestsResponse> GetUpcomingChests(string playerTag)
        {
            if (playerTag == null) throw new ArgumentNullException(nameof(playerTag));

            var cacheKey = CachePrefix + playerTag + "chests";
            if (_cache.TryGetValue(cacheKey, out var cached))
            {
                return JsonSerializer.Deserialize<GetUpcomingChestsResponse>((string)cached, _jsonOptions);
            }

            var decodedPlayerTag = playerTag.StartsWith("#")
                ? UrlEncoder.Default.Encode(playerTag)
                : $"%23{playerTag}";

            var requestUri = ApiPath + decodedPlayerTag + "/upcomingchests";
            var (player, raw) = await _apiClient.GetAsync<GetUpcomingChestsResponse>(requestUri);

            _cache.Set(cacheKey, raw);

            return player;
        }
    }
}
