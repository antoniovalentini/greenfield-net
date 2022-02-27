using System.Net.Http;
using System.Threading.Tasks;
using Avalentini.Shakesmon.Api.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Avalentini.Shakesmon.Api.IntegrationTests.Controllers
{
    public class PokemonControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;
        private const string PokemonUrl = "api/pokemon";

        public PokemonControllerTests(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _output = output;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetOperation_ShouldReturnTranslation()
        {
            // Arrange
            const string pokemonName = "charizard";

            // Act
            var response = await _client.GetAsync($"{PokemonUrl}/{pokemonName}");

            // Assert
            response.EnsureSuccessStatusCode();
            var dto = JsonConvert.DeserializeObject<GetDto>(await response.Content.ReadAsStringAsync());
            Assert.True(dto.Name.Equals(pokemonName));
            _output.WriteLine(dto.Description);
        }

        [Fact]
        public async Task GetOperation_ShouldReturnMessage_WhenNameNotSpecified()
        {
            // Act
            var response = await _client.GetAsync(PokemonUrl);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.True(content.Equals(PokemonController.ProvidePokemonNameMessage));
        }
    }
}
