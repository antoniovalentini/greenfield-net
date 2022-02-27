using System.Net.Http;
using System.Threading.Tasks;
using Avalentini.Shakesmon.Core.Services.PokeApi;
using Xunit;

namespace Avalentini.Shakesmon.Core.IntegrationTests.Services.PokeApi
{
    public class PokemonServiceTests
    {
        private readonly HttpClient _client;

        public PokemonServiceTests()
        {
            _client = new HttpClient();
        }

        [Fact]
        public async Task GetPokemon_ShouldReturnPokemon_WhenSuccess()
        {
            // ARRANGE
            const string name = "charizard";
            var sut = new PokemonService(_client);

            // ACT
            var result = await sut.GetPokemonAsync(name);

            // ASSERT
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Pokemon);
            Assert.False(string.IsNullOrEmpty(result.Pokemon.Id));
        }

        [Fact]
        public async Task GetSpecies_ShouldReturnPokemon_WhenSuccess()
        {
            // ARRANGE
            const string id = "1";
            var sut = new PokemonService(_client);

            // ACT
            var result = await sut.GetSpeciesFlavorTextAsync(id);

            // ASSERT
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.FlavorTextEntry);
            Assert.False(string.IsNullOrEmpty(result.FlavorTextEntry.FlavorText));
        }
    }
}
