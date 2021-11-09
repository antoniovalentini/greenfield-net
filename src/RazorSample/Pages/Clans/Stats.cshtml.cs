using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Royale.Sdk;
using Royale.Sdk.Clans.Models;

namespace RazorSample.Pages.Clans
{
    public class Stats : PageModel
    {
        private readonly IRoyaleClient _royaleClient;
        public string Error { get; private set; }
        public Clan Clan { get; private set; }
        public GetRiverRaceLogResponse RiverRaceLog { get; private set; }

        public Stats(IRoyaleClient royaleClient)
        {
            _royaleClient = royaleClient;
        }

        public async Task OnGet(string clanTag)
        {
            if (!string.IsNullOrWhiteSpace(Error) || string.IsNullOrWhiteSpace(clanTag))
            {
                return;
            }

            try
            {
                Clan = await _royaleClient.ClansApi.GetClan(clanTag);
                RiverRaceLog = await _royaleClient.ClansApi.GetRiverRaceLog(clanTag);
            }
            catch (RoyaleSdkException ex)
            {
                Error = ex.Message;
            }
        }
    }

    public record StatResult(string Name, int Count);
}
