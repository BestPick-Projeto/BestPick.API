using BestPick.Dominio.Enumeradores;

namespace BestPick.Dominio.Entidades;

public class Favorito
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public StatusFavorito Status { get; set; }
    public Produto Produto { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }
    public bool Ativo { get; set; }

    public Favorito()
    {
        Ativo = true;
    }
}