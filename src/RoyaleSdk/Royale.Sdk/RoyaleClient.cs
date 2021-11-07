using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Royale.Sdk.Clans;
using Royale.Sdk.Players;

namespace Royale.Sdk
{
    public class RoyaleClient : IRoyaleClient
    {
        public IClansApi ClansApi { get; }
        public IPlayersApi PlayersApi { get; }

        public RoyaleClient(IConfiguration config, IMemoryCache cache)
        {
            ClansApi = new ClansApi(config, cache);
            PlayersApi = new PlayesApi(config, cache);
        }
    }
}
