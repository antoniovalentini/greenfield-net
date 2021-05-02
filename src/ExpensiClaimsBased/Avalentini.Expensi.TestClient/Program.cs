using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Avalentini.Expensi.TestClient
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            // discover endpoints from metadata
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:49452");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            await RequestWithUsernameAndPassword(client, disco);
            Console.WriteLine("\n");
            await RequestWithClientCredentials(client, disco);

            Console.ReadKey();
        }

        private static async Task RequestWithClientCredentials(HttpMessageInvoker client, DiscoveryDocumentResponse disco)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Request using Client credentials");
            Console.ResetColor();

            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            });
            
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n");
            Console.WriteLine("Identity: ");

            // call api for identity
            await AskForIdentity(tokenResponse.AccessToken);
        }

        private static async Task RequestWithUsernameAndPassword(HttpMessageInvoker client, DiscoveryDocumentResponse disco)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Request using username and password");
            Console.ResetColor();

            // request token
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "ro.client",
                ClientSecret = "secret",

                UserName = "bob",
                Password = "password",
                Scope = "api1"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n");
            Console.WriteLine("Identity: ");

            // call api for identity
            await AskForIdentity(tokenResponse.AccessToken);
        }

        private static async Task AskForIdentity(string accessToken)
        {
            // call api
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(accessToken);

            try
            {
                var response = await apiClient.GetAsync("http://localhost:52504/identity");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(JArray.Parse(content));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
