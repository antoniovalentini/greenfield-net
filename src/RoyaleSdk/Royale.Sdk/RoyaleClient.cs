using Microsoft.Extensions.Caching.Memory;
using Royale.Sdk.Clans;
using Royale.Sdk.Players;

namespace Royale.Sdk
{
    public class RoyaleClient : IRoyaleClient
    {
        public IClansApi ClansApi { get; }
        public IPlayersApi PlayersApi { get; }

        public RoyaleClient(IMemoryCache cache, IApiClient apiClient)
        {
            ClansApi = new ClansApi(cache, apiClient);
            PlayersApi = new PlayesApi(cache, apiClient);
        }
    }
}
