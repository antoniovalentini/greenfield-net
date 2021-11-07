using Royale.Sdk.Clans;

namespace Royale.Sdk
{
    public interface IRoyaleClient
    {
        IClansApi ClansApi { get; }
    }
}
