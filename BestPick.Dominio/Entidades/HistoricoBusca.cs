namespace BestPick.Dominio.Entidades;

public class HistoricoBusca
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int ProdutoId { get; set; }
    public string MotivoCompra { get; set; }
    public decimal ValorCompra { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataBusca { get; set; }
    public Usuario Usuario { get; set; }
    public Produto Produto { get; set; }
}