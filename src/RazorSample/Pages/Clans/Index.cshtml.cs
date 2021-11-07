using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using RazorSample.Pages.Clans.Models;

namespace RazorSample.Pages.Clans
{
    public class Index : PageModel
    {
        private readonly string _token;

        public string Error { get; private set; }
        public Clan Clan { get; private set; }

        private readonly JsonSerializerOptions _jsonOptions = new() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

        public Index(IConfiguration config)
        {
            _token = config["ApiToken"];
            if (string.IsNullOrWhiteSpace(_token))
            {
                Error = "Api Token not found. Please define an 'ApiToken' property in the appsettings.json.";
            }
        }

        public async Task OnPost(string clanTag)
        {
            if (!string.IsNullOrWhiteSpace(Error) || string.IsNullOrWhiteSpace(clanTag))
            {
                return;
            }

            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.clashroyale.com/v1/clans/")
            };

            clanTag = clanTag.StartsWith("#") ? UrlEncoder.Default.Encode(clanTag) : $"%23{clanTag}";

            var request = new HttpRequestMessage(HttpMethod.Get, clanTag);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Error = content;
                return;
            }

            Clan = JsonSerializer.Deserialize<Clan>(content, _jsonOptions);
        }
    }
}
