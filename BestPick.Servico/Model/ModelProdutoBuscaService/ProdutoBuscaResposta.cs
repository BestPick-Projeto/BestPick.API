namespace BestPick.Servicos.Models;

public class ProdutoBuscaResposta
{
    public string Id { get; set; }

    public string Nome { get; set; }

    public decimal Preco { get; set; }

    public string Loja { get; set; }

    public string Url { get; set; }

    public string ImgUrl { get; set; }

    public decimal? Avaliacao { get; set; }

    public int Avaliacoes { get; set; }
}