using System.Threading.Tasks;
using Avalentini.Expensi.IdentityServer.Data.Entities;

namespace Avalentini.Expensi.IdentityServer.Data.Repository
{
    public interface IUserRepository
    {
        Task<UserEntity> FindAsync(string usernameOrEmail);
    }
}