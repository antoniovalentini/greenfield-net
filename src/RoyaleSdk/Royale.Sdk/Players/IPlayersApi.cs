using System.Threading.Tasks;
using Royale.Sdk.Players.Models;

namespace Royale.Sdk.Players
{
    public interface IPlayersApi
    {
        Task<Player> GetPlayer(string playerTag);
        Task<GetUpcomingChestsResponse> GetUpcomingChests(string playerTag);
    }
}
