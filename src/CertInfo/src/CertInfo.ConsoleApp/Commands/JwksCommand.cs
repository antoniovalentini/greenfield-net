using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CertInfo.ConsoleApp.Parsers;
using Spectre.Console.Cli;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace CertInfo.ConsoleApp.Commands
{
    public class JwksCommand : AsyncCommand<JwksCommand.Settings>
    {
        public sealed class Settings : CommandSettings
        {
            [Description("Path to the JWKs file.")]
            [CommandArgument(0, "<jwksPath>")]
            public string? JwkPath { get; init; }
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.JwkPath))
            {
                throw new Exception("Path to the JWKs file.");
            }
            
            var jwks = await FetchJwks(settings.JwkPath);

            foreach (var key in jwks.Keys)
            {
                var (isSuccess, output) = await new WebParser().TryParseAsync(key.X5U);
                Helpers.PrintOutput(key.X5U, isSuccess, output);
            }
            
            return 0;
        }

        private static async Task<Jwks> FetchJwks(string path)
        {
            if (path.StartsWith("http"))
            {
                using var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(path),
                };
                return await httpClient.GetFromJsonAsync<Jwks>("") 
                       ?? throw new Exception("Unable to fetch jwks from the network.");
            }
            
            var raw = await File.ReadAllTextAsync(path);
            return JsonSerializer.Deserialize<Jwks>(raw) 
                   ?? throw new Exception("Unable to fetch jwks locally.");
        }
    }

    public record Jwks([property:JsonPropertyName("keys")] Key[] Keys);

    public record Key(
        [property:JsonPropertyName("kid")] string Kid,
        [property:JsonPropertyName("kty")] string Kty,
        [property:JsonPropertyName("n")] string N,
        [property:JsonPropertyName("e")] string E,
        [property:JsonPropertyName("use")]string Use,
        [property:JsonPropertyName("x5c")] IEnumerable X5C,
        [property:JsonPropertyName("x5u")] string X5U,
        [property:JsonPropertyName("x5t")] string X5T,
        [property:JsonPropertyName("x5t#S256")] string X5Ts256);
}