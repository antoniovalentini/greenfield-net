using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Royale.Sdk.Clans;

namespace Royale.Sdk
{
    public class RoyaleClient : IRoyaleClient
    {
        public IClansApi ClansApi { get; }

        public RoyaleClient(IConfiguration config, IMemoryCache cache)
        {
            ClansApi = new ClansApi(config, cache);
        }
    }
}
