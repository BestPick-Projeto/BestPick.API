namespace BestPick.Api.Models;

public class ChatRequisicao
{
    public string Mensagem { get; set; }

    public string Produto { get; set; }

    public decimal? ValorCompra { get; set; }

    public string MotivoCompra { get; set; }
}