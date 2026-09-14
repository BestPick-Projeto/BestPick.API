using Newtonsoft.Json;

namespace BestPick.Servicos.Models;

public class CompoundCustom
{
    [JsonProperty("tools")]
    public CompoundTools Tools { get; set; }
}

public class CompoundTools
{
    [JsonProperty("enabled_tools")]
    public List<string> EnabledTools { get; set; }
}