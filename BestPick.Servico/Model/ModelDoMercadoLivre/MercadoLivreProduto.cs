using Newtonsoft.Json;

namespace BestPick.Servicos.Models;

public class MercadoLivreProduto
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("permalink")]
    public string Permalink { get; set; }

    [JsonProperty("thumbnail")]
    public string Thumbnail { get; set; }

    [JsonIgnore]
    public decimal Preco { get; set; }
}