using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Avalentini.Shakesmon.Core.Services.PokeApi;
using Avalentini.Shakesmon.Core.Services.PokeApi.Dto;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace Avalentini.Shakesmon.Core.UnitTests.Services.PokeApi
{
    public class PokemonServiceTests
    {
        #region GET_POKEMON
        [Fact]
        public async Task GetPokemon_ShouldReturnPokemon_WhenHttpClientReturnsOk()
        {
            // ARRANGE
            var pokemon = new Pokemon {Id = "6"};
            var client = MockHttpClientGetPokemon(HttpStatusCode.OK, pokemon);
            var sut = new PokemonService(client);

            // ACT
            var result = await sut.GetPokemonAsync("anything");

            // ASSERT
            Assert.NotNull(result);
            Assert.NotNull(result.Pokemon);
            Assert.True(result.Pokemon.Id == "6");
        }

        [Fact]
        public async Task GetPokemon_ShouldReturnError_WhenHttpClientReturnsError()
        {
            // ARRANGE
            var pokemon = new Pokemon {Id = "6"};
            var client = MockHttpClientGetPokemon(HttpStatusCode.InternalServerError, pokemon);
            var sut = new PokemonService(client);

            // ACT
            var result = await sut.GetPokemonAsync("anything");

            // ASSERT
            Assert.NotNull(result);
            Assert.Null(result.Pokemon);
            Assert.True(result.Error.Equals(PokemonService.PokemonInvalidResponseError, StringComparison.InvariantCultureIgnoreCase));
        }

        [Fact]
        public async Task GetPokemon_ShouldReturnError_WhenNoNameIsProvided()
        {
            // ARRANGE
            var pokemon = new Pokemon {Id = "6"};
            var client = MockHttpClientGetPokemon(HttpStatusCode.InternalServerError, pokemon);
            var sut = new PokemonService(client);

            // ACT
            var result = await sut.GetPokemonAsync("");

            // ASSERT
            Assert.NotNull(result);
            Assert.Null(result.Pokemon);
            Assert.True(result.Error.Equals(PokemonService.EmptyPokemonNameError, StringComparison.InvariantCultureIgnoreCase));
        }

        [Fact]
        public async Task GetPokemon_ShouldReturnNotFoundError_WhenPokemonNotFound()
        {
            // ARRANGE
            var pokemon = new Pokemon {Id = "6"};
            var client = MockHttpClientGetPokemon(HttpStatusCode.NotFound, pokemon);
            var sut = new PokemonService(client);

            // ACT
            var result = await sut.GetPokemonAsync("anything");

            // ASSERT
            Assert.NotNull(result);
            Assert.Null(result.Pokemon);
            Assert.True(result.Error.Equals(PokemonService.PokemonNotFoundError, StringComparison.InvariantCultureIgnoreCase));
        }

        private static HttpClient MockHttpClientGetPokemon(HttpStatusCode status, Pokemon pokemon)
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = status,
                    Content = new StringContent(JsonConvert.SerializeObject(pokemon)),
                });
            return new HttpClient(handler.Object);
        }
        #endregion

        #region GET_SPECIES
        [Fact]
        public async Task GetSpecies_ShouldReturnSpecies_WhenHttpClientReturnsOk()
        {
            // ARRANGE
            const string flavorText = "something";
            var species = MockSpecies(flavorText);
            var client = MockHttpClientGetSpecies(HttpStatusCode.OK, species);
            var sut = new PokemonService(client);

            // ACT
            var result = await sut.GetSpeciesFlavorTextAsync("anything");

            // ASSERT
            Assert.NotNull(result);
            Assert.NotNull(result.FlavorTextEntry);
            Assert.True(result.FlavorTextEntry.FlavorText.Equals(flavorText, StringComparison.InvariantCultureIgnoreCase));
        }

        [Fact]
        public async Task GetSpecies_ShouldReturnError_WhenHttpClientReturnsError()
        {
            // ARRANGE
            const string flavorText = "something";
            var species = MockSpecies(flavorText);
            var client = MockHttpClientGetSpecies(HttpStatusCode.InternalServerError, species);
            var sut = new PokemonService(client);

            // ACT
            var result = await sut.GetSpeciesFlavorTextAsync("anything");

            // ASSERT
            Assert.NotNull(result);
            Assert.Null(result.FlavorTextEntry);
            Assert.True(result.Error.Equals(PokemonService.SpeciesInvalidResponseError, StringComparison.InvariantCultureIgnoreCase));
        }

        [Fact]
        public async Task GetSpecies_ShouldReturnError_WhenNoIdIsProvided()
        {
            // ARRANGE
            const string flavorText = "something";
            var species = MockSpecies(flavorText);
            var client = MockHttpClientGetSpecies(HttpStatusCode.InternalServerError, species);
            var sut = new PokemonService(client);

            // ACT
            var result = await sut.GetSpeciesFlavorTextAsync("");

            // ASSERT
            Assert.NotNull(result);
            Assert.Null(result.FlavorTextEntry);
            Assert.True(result.Error.Equals(PokemonService.EmptyPokemonIdError, StringComparison.InvariantCultureIgnoreCase));
        }

        [Fact]
        public async Task GetSpecies_ShouldReturnNotFoundError_WhenPokemonNotFound()
        {
            // ARRANGE
            const string flavorText = "something";
            var species = MockSpecies(flavorText);
            var client = MockHttpClientGetSpecies(HttpStatusCode.NotFound, species);
            var sut = new PokemonService(client);

            // ACT
            var result = await sut.GetSpeciesFlavorTextAsync("anything");

            // ASSERT
            Assert.NotNull(result);
            Assert.Null(result.FlavorTextEntry);
            Assert.True(result.Error.Equals(PokemonService.SpeciesNotFoundError, StringComparison.InvariantCultureIgnoreCase));
        }

        private static Species MockSpecies(string flavorText)
        {
            return new Species {FlavorTextEntries = new List<FlavorTextEntry>
            {
                new FlavorTextEntry
                {
                    FlavorText = flavorText,
                    Language = new Language {Name = "en"}
                }
            }};
        }

        private static HttpClient MockHttpClientGetSpecies(HttpStatusCode status, Species species)
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = status,
                    Content = new StringContent(JsonConvert.SerializeObject(species)),
                });
            return new HttpClient(handler.Object);
        }
        #endregion
    }
}
