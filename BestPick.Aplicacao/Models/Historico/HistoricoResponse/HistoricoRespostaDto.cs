namespace BestPick.Aplicacao.Models;

public class HistoricoRespostaDto
{
    public int Id { get; set; }

    public decimal ValorCompra { get; set; }

    public string MotivoCompra { get; set; }

    public string ProdutoNome { get; set; }

    public DateTime DataBusca { get; set; }
}