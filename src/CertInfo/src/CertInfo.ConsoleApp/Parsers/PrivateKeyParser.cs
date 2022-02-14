using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CertInfo.ConsoleApp.Parsers
{
    public class PrivateKeyParser : ISecretParser
    {
        public async Task<(bool IsSuccess, SecretOutput Output)> TryParseAsync(string filePath)
        {
            try
            {
                var rsa = RSA.Create();
                var keyRaw = await File.ReadAllTextAsync(filePath);
                keyRaw = CleanBase64String(keyRaw);
                var source = Convert.FromBase64String(keyRaw);
                var output = new EmptyOutput();
                try
                {
                    rsa.ImportPkcs8PrivateKey(source, out _);
                    output = output with { Type = "Pkcs8PrivateKey" };
                }
                catch { /**/ }
                try
                {
                    rsa.ImportRSAPrivateKey(source, out _);
                    output = output with { Type = "RSAPrivateKey" };
                }
                catch { /**/ }
                try
                {
                    rsa.ImportRSAPublicKey(source, out _);
                    output = output with { Type = "RSAPublicKey" };
                }
                catch { /**/ }
                return (!string.IsNullOrWhiteSpace(output.Type), output);
            }
            catch
            {
                return (false, new EmptyOutput());
            }
        }

        private static string CleanBase64String(string base64)
        {
            if (base64 == null) throw new ArgumentNullException(nameof(base64));

            return base64
                    .Replace("-----BEGIN PRIVATE KEY-----", "")
                    .Replace("-----BEGIN RSA PRIVATE KEY-----", "")
                    .Replace("-----END RSA PRIVATE KEY-----", "")
                    .Replace("-----END PRIVATE KEY-----", "")
                    .Replace("\n", "");
        }
    }
}
