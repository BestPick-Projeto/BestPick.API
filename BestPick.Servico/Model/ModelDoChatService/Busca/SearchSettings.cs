using Newtonsoft.Json;

namespace BestPick.Servicos.Models;

public class SearchSettings
{
    [JsonProperty("include_domains")]
    public List<string> IncludeDomains { get; set; }

    [JsonProperty("country")]
    public string Country { get; set; }
}