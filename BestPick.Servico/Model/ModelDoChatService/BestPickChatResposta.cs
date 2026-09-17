namespace BestPick.Servicos.Models;

public class BestPickChatResposta
{
    public bool Completo { get; set; }

    public string Produto { get; set; }

    public decimal? ValorCompra { get; set; }

    public string MotivoCompra { get; set; }

    public string Mensagem { get; set; }

    public List<BestPickProdutoResposta> Produtos { get; set; }
}