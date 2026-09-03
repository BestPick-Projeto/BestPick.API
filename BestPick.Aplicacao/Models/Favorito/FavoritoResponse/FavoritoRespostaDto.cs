namespace BestPick.Aplicacao.Models;

public class FavoritoRespostaDto
{
    public int Id { get; set; }
    public string NomeProduto { get; set; }
    public string Categoria { get; set; }
    public decimal Preco { get; set; }
    public string ImgUrl { get; set; }
    public string Url { get; set; }
    public List<string> Beneficios { get; set; }
    public List<string> Maleficios { get; set; }
    public string Status { get; set; }
}