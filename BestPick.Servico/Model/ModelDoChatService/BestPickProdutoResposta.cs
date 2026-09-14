namespace BestPick.Servicos.Models;

public class BestPickProdutoResposta
{
    public string Nome { get; set; }

    public string Categoria { get; set; }

    public decimal Preco { get; set; }

    public string ImgUrl { get; set; }

    public string Url { get; set; }

    public List<string> Beneficios { get; set; }

    public List<string> Maleficios { get; set; }
}