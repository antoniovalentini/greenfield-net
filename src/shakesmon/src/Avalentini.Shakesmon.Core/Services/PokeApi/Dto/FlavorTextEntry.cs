using Newtonsoft.Json;

namespace Avalentini.Shakesmon.Core.Services.PokeApi.Dto
{
    public class FlavorTextEntry
    {
        [JsonProperty("flavor_text")]
        public string FlavorText { get; set; }

        public Language Language { get; set; }
    }
}
