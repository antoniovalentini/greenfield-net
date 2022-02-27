using Avalentini.Shakesmon.Core.Services.FunTranslations;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Avalentini.Shakesmon.Core.Services.FunTranslations.Dto;
using Xunit;
using System;

namespace Avalentini.Shakesmon.Core.UnitTests.Services.FunTranslations
{
    public class ShakespeareServiceTests
    {
        [Fact]
        public async Task Translate_ShouldReturnTranslation_WhenHttpClientReturnsOk()
        {
            // ARRANGE
            const string translated = "someText";
            var translation = new Translation {Contents = new Contents {Translated = translated}};
            var client = MockHttpClientTranslate(HttpStatusCode.OK, translation);
            var sut = new ShakespeareService(client);

            // ACT
            var result = await sut.Translate("anything");

            // ASSERT
            Assert.NotNull(result);
            Assert.NotNull(result.Translation);
            Assert.NotNull(result.Translation.Contents);
            Assert.True(result.Translation.Contents.Translated.Equals(translated));
        }

        [Fact]
        public async Task Translate_ShouldReturnError_WhenHttpClientReturnsError()
        {
            // ARRANGE
            var client = MockHttpClientTranslate(HttpStatusCode.InternalServerError, new Translation());
            var sut = new ShakespeareService(client);

            // ACT
            var result = await sut.Translate("anything");

            // ASSERT
            Assert.NotNull(result);
            Assert.Null(result.Translation);
            Assert.True(result.Error.Equals(ShakespeareService.ShakespeareInvalidResponseError, StringComparison.InvariantCultureIgnoreCase));
        }

        [Fact]
        public async Task Translate_ShouldReturnError_WhenTextIsNotProvided()
        {
            // ARRANGE
            var client = MockHttpClientTranslate(HttpStatusCode.OK, new Translation());
            var sut = new ShakespeareService(client);

            // ACT
            var result = await sut.Translate("");

            // ASSERT
            Assert.NotNull(result);
            Assert.Null(result.Translation);
            Assert.True(result.Error.Equals(ShakespeareService.EmptyTranslationError, StringComparison.InvariantCultureIgnoreCase));
        }

        private static HttpClient MockHttpClientTranslate(HttpStatusCode status, Translation translation)
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = status,
                    Content = new StringContent(JsonConvert.SerializeObject(translation)),
                });
            return new HttpClient(handler.Object);
        }
    }
}
