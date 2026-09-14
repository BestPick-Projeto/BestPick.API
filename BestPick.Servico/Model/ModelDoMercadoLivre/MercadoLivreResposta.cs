using Newtonsoft.Json;

namespace BestPick.Servicos.Models;

public class MercadoLivreResposta
{
    [JsonProperty("results")]
    public List<MercadoLivreProduto> Results { get; set; }
}