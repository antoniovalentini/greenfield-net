using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CertInfo.ConsoleApp.Parsers;
using Spectre.Console;

namespace CertInfo.ConsoleApp
{
    public static class Helpers
    {
        public static string CalculateKid(this X509Certificate2 cert)
        {
            if (cert == null) throw new ArgumentNullException(nameof(cert));

            return Convert.ToBase64String(cert.GetCertHash())
                .Replace("+", "-")
                .Replace("=", "")
                .Replace("/", "_");
        }
        
        private static void PrintDetail(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }
            AnsiConsole.MarkupLine($"[bold gray]  {key}[/]: {value}");
        }
 
        public static void PrintOutput(string path, bool isSuccess, SecretOutput output)
        {
            if (isSuccess) {
                var fileName = Path.GetFileName(path)
                    .Replace(output.ThumbSha256, "")
                    .TrimEnd('_');
                AnsiConsole.MarkupLine($"[bold yellow]{fileName} [[{output.OrganizationIdentifier()}]][/]");
                PrintDetail("type", output.Type);
                PrintDetail("kid", output.Kid);
                PrintDetail("thumbprint", output.Thumbprint);
                PrintDetail("serial", output.SerialNumber);
            }
            else
            {
                AnsiConsole.MarkupLine($"[bold yellow]{Path.GetFileName(path)}[/]");
                PrintDetail("type", "Unknown");
            }
        }

        private static string OrganizationIdentifier(this SecretOutput output)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));

            if (string.IsNullOrWhiteSpace(output.SubjectName)) return string.Empty;

            var tuple = output.SubjectName
                .Split(", ")
                .FirstOrDefault(x => x.Contains("organizationIdentifier", StringComparison.InvariantCultureIgnoreCase));

            return string.IsNullOrWhiteSpace(tuple)
                ? string.Empty 
                : tuple.Split("=")[1];
        }
    }
}
