using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CertInfo.ConsoleApp.Parsers
{
    public class WebParser : ISecretParser
    {
        public async Task<(bool IsSuccess, SecretOutput Output)> TryParseAsync(string filePath)
        {
            if (!filePath.StartsWith("http"))
            {
                return (false, new EmptyOutput());
            }

            using var httpclient = new HttpClient
            {
                BaseAddress = new Uri(filePath),
            };

            var response = await httpclient.GetAsync(string.Empty);

            if (!response.IsSuccessStatusCode)
            {
                return (false, new EmptyOutput());
            }
            
            var content = await response.Content.ReadAsStringAsync();

            var cleanCertBase64 = content
                .Replace("-----BEGIN CERTIFICATE-----", "")
                .Replace("-----END CERTIFICATE-----", "")
                .Replace("\n", "")
                .Replace("\r", "");

            var rawData = Convert.FromBase64String(cleanCertBase64);

            var cert = new X509Certificate2(rawData);
            
            var thumbSha256 = Convert.ToHexString(SHA256.HashData(cert.RawData));

            var kid = cert.CalculateKid();

            var output = new SecretOutput(
                "certificate", 
                cert.SubjectName.Name, 
                kid, cert.Thumbprint, 
                cert.SerialNumber,
                thumbSha256);

            return (true, output);
        }
    }
}
