using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CertInfo.ConsoleApp.Parsers
{
    public class CertificateParser : ISecretParser
    {
        public async Task<(bool IsSuccess, SecretOutput Output)> TryParseAsync(string filePath)
        {
            try
            {
                var cert = new X509Certificate2(filePath);
                var kid = cert.CalculateKid();
                var thumbSha256 = Convert.ToHexString(SHA256.HashData(cert.RawData));
                var output = new SecretOutput("certificate", cert.SubjectName.Name, kid, cert.Thumbprint, cert.SerialNumber, thumbSha256);
                return await Task.FromResult((true, output));
            }
            catch
            {
                return (false, new EmptyOutput());
            }
        }
    }
}
