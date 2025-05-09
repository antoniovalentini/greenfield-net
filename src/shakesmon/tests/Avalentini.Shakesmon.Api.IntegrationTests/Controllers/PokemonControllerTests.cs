﻿using System.Net.Http;
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

        [Fact(Skip = "Mock the HttpClient")]
        public async Task GetOperation_ShouldReturnTranslation()
        {
            // Arrange
            const string pokemonName = "charizard";

            // Act
            var response = await _client.GetAsync($"{PokemonUrl}/{pokemonName}");

            // Assert
            response.EnsureSuccessStatusCode();
            var dto = JsonConvert.DeserializeObject<GetDto>(await response.Content.ReadAsStringAsync());
            Assert.Equal(pokemonName, dto.Name);
            _output.WriteLine(dto.Description);
        }

        [Fact(Skip = "Mock the HttpClient")]
        public async Task GetOperation_ShouldReturnMessage_WhenNameNotSpecified()
        {
            // Act
            var response = await _client.GetAsync(PokemonUrl);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Equal(PokemonController.ProvidePokemonNameMessage, content);
        }
    }
}
