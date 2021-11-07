using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Royale.Sdk;
using Royale.Sdk.Clans.Models;

namespace RazorSample.Pages.Clans
{
    public class Index : PageModel
    {
        private readonly IRoyaleClient _royaleClient;
        public string Error { get; private set; }
        public Clan Clan { get; private set; }

        public Index(IRoyaleClient royaleClient)
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
            }
            catch (RoyaleSdkException ex)
            {
                Error = ex.Message;
            }
        }
    }
}
