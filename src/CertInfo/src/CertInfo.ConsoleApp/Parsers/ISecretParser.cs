using System.Threading.Tasks;

namespace CertInfo.ConsoleApp.Parsers
{
    public interface ISecretParser
    {
        Task<(bool IsSuccess, SecretOutput Output)> TryParseAsync(string filePath);
    }
}
