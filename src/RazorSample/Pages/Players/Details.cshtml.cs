using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Royale.Sdk;
using Royale.Sdk.Players.Models;

namespace RazorSample.Pages.Players
{
    public class Details : PageModel
    {
        private readonly IRoyaleClient _royaleClient;

        public Player Player { get; private set; }
        public string Error { get; private set; }

        public Details(IRoyaleClient royaleClient)
        {
            _royaleClient = royaleClient;
        }

        public async Task OnGet(string playerTag)
        {
            if (!string.IsNullOrWhiteSpace(Error) || string.IsNullOrWhiteSpace(playerTag))
            {
                return;
            }

            try
            {
                Player = await _royaleClient.PlayersApi.GetPlayer(playerTag);
            }
            catch (RoyaleSdkException ex)
            {
                Error = ex.Message;
            }
        }
    }
}
