using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalentini.Shakesmon.Core.Services.FunTranslations.Dto;
using Newtonsoft.Json;

namespace Avalentini.Shakesmon.Core.Services.FunTranslations
{
    public interface IShakespeareService
    {
        Task<TranslateResponse> Translate(string text);
    }

    public class ShakespeareService : IShakespeareService
    {
        private readonly HttpClient _client;
        private const string ShakespeareApiBaseUrl = "https://api.funtranslations.com/translate/";
        private const string ShakespeareRequestUri = "shakespeare.json";
        public const string ShakespeareInvalidResponseError = "Unable to translate text.";
        public const string EmptyTranslationError = "Provided translation text is empty.";

        public ShakespeareService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(ShakespeareApiBaseUrl);
        }

        public async Task<TranslateResponse> Translate(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new TranslateResponse{Error = EmptyTranslationError};

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("text", Regex.Replace(text, @"\n|\f", " "))
            });
            var shakeResponse = await _client.PostAsync(ShakespeareRequestUri, content);
            // TODO: handle requests limit reached
            if (!shakeResponse.IsSuccessStatusCode)
                return new TranslateResponse{Error = ShakespeareInvalidResponseError};
            var translation = JsonConvert.DeserializeObject<Translation>(await shakeResponse.Content.ReadAsStringAsync());
            return new TranslateResponse {Translation = translation};
        }
    }
}
