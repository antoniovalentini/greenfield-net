using System.Net.Http;
using System.Threading.Tasks;
using Avalentini.Shakesmon.Core.Services.FunTranslations;
using Xunit;

namespace Avalentini.Shakesmon.Core.IntegrationTests.Services.FunTranslations
{
    public class ShakespeareServiceTests
    {
        private readonly HttpClient _client;

        public ShakespeareServiceTests()
        {
            _client = new HttpClient();
        }

        [Fact]
        public async Task Translate_ShouldReturnTranslation_WhenSuccess()
        {
            // ARRANGE
            const string text = "The quick brown fox jumps over the lazy dog";
            var sut = new ShakespeareService(_client);

            // ACT
            var result = await sut.Translate(text);

            // ASSERT
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Translation);
            Assert.NotNull(result.Translation.Contents);
            Assert.False(string.IsNullOrEmpty(result.Translation.Contents.Translation));
        }
    }
}
