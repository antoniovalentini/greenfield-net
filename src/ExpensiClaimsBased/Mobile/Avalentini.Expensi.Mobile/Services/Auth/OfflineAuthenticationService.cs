using System.Threading;
using System.Threading.Tasks;

namespace Avalentini.Expensi.Mobile.Services.Auth
{
    public class OfflineAuthenticationService : IAuthenticationService
    {
        public async Task<bool> TrySilentLoginAsync(CancellationToken cancellationToken)
        {
            return await Task.FromResult(false);
        }

        public async Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken)
        {
            return await Task.FromResult(username == "anto" && password == "test");
        }
    }
}
