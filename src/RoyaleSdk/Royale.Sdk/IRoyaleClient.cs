using Royale.Sdk.Clans;
using Royale.Sdk.Players;

namespace Royale.Sdk
{
    public interface IRoyaleClient
    {
        IClansApi ClansApi { get; }
        IPlayersApi PlayersApi { get; }
    }
}
