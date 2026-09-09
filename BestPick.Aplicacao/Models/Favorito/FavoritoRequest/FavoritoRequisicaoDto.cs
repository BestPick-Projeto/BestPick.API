using BestPick.Dominio.Entidades;
using BestPick.Dominio.Enumeradores;

namespace BestPick.Aplicacao.Models;

public class FavoritoCriar
{
    public int ProdutoId { get; set; }
}

public class FavoritoAtualizar
{
    public int FavoritoId { get; set; }
    public StatusFavorito NovoStatus { get; set; }
}