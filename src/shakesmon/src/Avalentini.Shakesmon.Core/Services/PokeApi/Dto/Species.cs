using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Avalentini.Shakesmon.Core.Services.PokeApi.Dto
{
    public class Species
    {
        [JsonProperty("flavor_text_entries")]
        public List<FlavorTextEntry> FlavorTextEntries { get; set; }

        public FlavorTextEntry GetFlavorByLanguage(string lang)
        {
            return string.IsNullOrEmpty(lang)
                ? null 
                : FlavorTextEntries.First(s => s.Language.Name.Equals(lang, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
