using System;
using System.Threading.Tasks;
using Avalentini.Shakesmon.Core.Features.PokemonTranslator;
using Avalentini.Shakesmon.Core.Services.FunTranslations;
using Avalentini.Shakesmon.Core.Services.FunTranslations.Dto;
using Avalentini.Shakesmon.Core.Services.PokeApi;
using Avalentini.Shakesmon.Core.Services.PokeApi.Dto;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace Avalentini.Shakesmon.Core.UnitTests.Services.Features.PokemonTranslator
{
    public class PokemonTranslatorFeatureTest
    {
        private static IMemoryCache MockValidCache()
        {
            var cache = new Mock<IMemoryCache>();
            object value;
            cache
                .Setup(c => c.TryGetValue(It.IsAny<string>(), out value))
                .Returns(false);
            var entry = new Mock<ICacheEntry>();
            cache
                .Setup(c => c.CreateEntry(It.IsAny<object>()))
                .Returns(entry.Object);

            return cache.Object;
        }

        [Fact]
        public async Task Execute_ShouldReturnTranslation_WhenSuccess()
        {
            // ARRANGE
            var cache = MockValidCache();

            var pokemonService = new Mock<IPokemonService>();
            pokemonService
                .Setup(ps => ps.GetPokemonAsync(It.IsAny<string>()))
                .ReturnsAsync(new GetPokemonResponse{Pokemon = new Pokemon{Id = "anything"}});
            pokemonService
                .Setup(ps => ps.GetSpeciesFlavorTextAsync(It.IsAny<string>()))
                .ReturnsAsync(new GetSpeciesResponse {FlavorTextEntry = new FlavorTextEntry {FlavorText = "anything"}});

            const string translated = "translated";
            var shakespeareService = new Mock<IShakespeareService>();
            shakespeareService
                .Setup(ss => ss.Translate(It.IsAny<string>()))
                .ReturnsAsync(new TranslateResponse{Translation = new Translation{Contents = new Contents{Translated = translated}}});

            // ACT
            var sut = new PokemonTranslatorFeature(pokemonService.Object, shakespeareService.Object, cache);
            var result = await sut.ExecuteAsync("anything");

            // ASSERT
            Assert.NotNull(result);
            Assert.True(result.Description.Equals(translated, StringComparison.InvariantCultureIgnoreCase));
        }

        [Fact]
        public async Task Execute_ShouldReturnTranslation_WhenCacheHit()
        {
            // ARRANGE
            const string translated = "translated";
            var cache = new Mock<IMemoryCache>();
            object value = translated;
            cache
                .Setup(c => c.TryGetValue(It.IsAny<string>(), out value))
                .Returns(true);

            // ACT
            var sut = new PokemonTranslatorFeature(null, null, cache.Object);
            var result = await sut.ExecuteAsync("anything");

            // ASSERT
            Assert.NotNull(result);
            Assert.True(result.Description.Equals(translated, StringComparison.InvariantCultureIgnoreCase));
        }

        [Fact]
        public async Task Execute_ShouldReturnError_WhenGetPokemonFails()
        {
            // ARRANGE
            var cache = MockValidCache();
            var pokemonService = new Mock<IPokemonService>();
            pokemonService
                .Setup(ps => ps.GetPokemonAsync(It.IsAny<string>()))
                .ReturnsAsync(new GetPokemonResponse{Error = "get pokemon error"});

            // ACT
            var sut = new PokemonTranslatorFeature(pokemonService.Object, null, cache);
            var result = await sut.ExecuteAsync("anything");

            // ASSERT
            Assert.NotNull(result);
            Assert.True(result.Error.Equals("get pokemon error", StringComparison.InvariantCultureIgnoreCase));
        }

        [Fact]
        public async Task Execute_ShouldReturnError_WhenGetSpeciesFails()
        {
            // ARRANGE
            var cache = MockValidCache();
            var pokemonService = new Mock<IPokemonService>();
            pokemonService
                .Setup(ps => ps.GetPokemonAsync(It.IsAny<string>()))
                .ReturnsAsync(new GetPokemonResponse{Pokemon = new Pokemon{Id = "anything"}});
            pokemonService
                .Setup(ps => ps.GetSpeciesFlavorTextAsync(It.IsAny<string>()))
                .ReturnsAsync(new GetSpeciesResponse {Error = "get species error"});

            // ACT
            var sut = new PokemonTranslatorFeature(pokemonService.Object, null, cache);
            var result = await sut.ExecuteAsync("anything");

            // ASSERT
            Assert.NotNull(result);
            Assert.True(result.Error.Equals("get species error", StringComparison.InvariantCultureIgnoreCase));
        }

        [Fact]
        public async Task Execute_ShouldReturnError_WhenTranslateFails()
        {
            // ARRANGE
            var cache = MockValidCache();

            var pokemonService = new Mock<IPokemonService>();
            pokemonService
                .Setup(ps => ps.GetPokemonAsync(It.IsAny<string>()))
                .ReturnsAsync(new GetPokemonResponse{Pokemon = new Pokemon{Id = "anything"}});
            pokemonService
                .Setup(ps => ps.GetSpeciesFlavorTextAsync(It.IsAny<string>()))
                .ReturnsAsync(new GetSpeciesResponse {FlavorTextEntry = new FlavorTextEntry {FlavorText = "anything"}});

            var shakespeareService = new Mock<IShakespeareService>();
            shakespeareService
                .Setup(ss => ss.Translate(It.IsAny<string>()))
                .ReturnsAsync(new TranslateResponse{Error = "translate error"});

            // ACT
            var sut = new PokemonTranslatorFeature(pokemonService.Object, shakespeareService.Object, cache);
            var result = await sut.ExecuteAsync("anything");

            // ASSERT
            Assert.NotNull(result);
            Assert.True(result.Error.Equals("translate error", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
