using System.Threading.Tasks;
using Avalentini.Shakesmon.Core.Common;
using Avalentini.Shakesmon.Core.Services.FunTranslations;
using Avalentini.Shakesmon.Core.Services.PokeApi;
using Microsoft.Extensions.Caching.Memory;

namespace Avalentini.Shakesmon.Core.Features.PokemonTranslator
{
    public class PokemonTranslatorFeature
    {
        private readonly IPokemonService _pokemonService;
        private readonly IShakespeareService _shakespeareService;
        private readonly IMemoryCache _cache;

        public PokemonTranslatorFeature(IPokemonService pokemonService, IShakespeareService shakespeareService, IMemoryCache cache)
        {
            _pokemonService = pokemonService;
            _shakespeareService = shakespeareService;
            _cache = cache;
        }

        public async Task<ExecuteResult> ExecuteAsync(string name)
        {
            var key = ComputeKey(name);
            var desc = _cache.Get<string>(key);
            if (!string.IsNullOrEmpty(desc))
                return new ExecuteResult {Description = desc};

            var pokeResult = await _pokemonService.GetPokemonAsync(name);
            if (!pokeResult.IsSuccess)
                return Error(pokeResult.Error);

            var speciesResult = await _pokemonService.GetSpeciesFlavorTextAsync(pokeResult.Pokemon.Id);
            if (!speciesResult.IsSuccess)
                return Error(speciesResult.Error);

            var translationResult = await _shakespeareService.Translate(speciesResult.FlavorTextEntry.FlavorText);
            if (!translationResult.IsSuccess)
                return Error(translationResult.Error);

            _cache.Set(key, translationResult.Translation.Contents.Translated);
            return new ExecuteResult {Description = translationResult.Translation.Contents.Translated};
        }

        private string ComputeKey(string name)
        {
            return $"pokemon_{name}";
        }

        private static ExecuteResult Error(string message)
        {
            return new ExecuteResult{Error = message};
        }
    }

    public class ExecuteResult : BaseResponse
    {
        public string Description { get; set; }
    }
}
