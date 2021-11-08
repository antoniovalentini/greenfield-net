using System.Threading.Tasks;

namespace Royale.Sdk
{
    public interface IApiClient
    {
        Task<ApiResponse<T>> GetAsync<T>(string requestUri);
    }
}
