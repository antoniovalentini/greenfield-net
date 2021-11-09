using System.Threading.Tasks;
using Royale.Sdk.Clans.Models;

namespace Royale.Sdk.Clans
{
    public interface IClansApi
    {
        Task<Clan> GetClan(string clanTag);
        Task<GetRiverRaceLogResponse> GetRiverRaceLog(string clanTag);
    }
}
