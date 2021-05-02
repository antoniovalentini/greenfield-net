using System.Threading;
using System.Threading.Tasks;

namespace Avalentini.Expensi.Mobile.Services.Auth
{
    public interface IAuthenticationService
    {
        //Task<bool> Init(CancellationToken cancellationToken);

        Task<bool> TrySilentLoginAsync(CancellationToken cancellationToken);
        Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken);
    }
}
