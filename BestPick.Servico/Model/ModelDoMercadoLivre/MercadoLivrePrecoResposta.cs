using Newtonsoft.Json;

namespace BestPick.Servicos.Models;

public class MercadoLivrePrecoResposta
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("prices")]
    public List<MercadoLivrePreco> Prices { get; set; }
}

public class MercadoLivrePreco
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("currency_id")]
    public string CurrencyId { get; set; }

    [JsonProperty("conditions")]
    public MercadoLivrePrecoCondicao Conditions { get; set; }
}

public class MercadoLivrePrecoCondicao
{
    [JsonProperty("context_restrictions")]
    public List<string> ContextRestrictions { get; set; }
}