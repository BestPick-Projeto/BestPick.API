using BestPick.Dominio.Entidades;
using BestPick.Dominio.Enumeradores;
namespace BestPick.Aplicacao.Interfaces;

public interface IFavoritoAplicacao
{
    Task<Favorito> AdicionarAsync(Favorito favorito);
    Task<Favorito> ObterPorIdAsync(int id);
    Task<IEnumerable<Favorito>> ListarPorUsuarioAsync(int usuarioId);
    Task AtualizarStatusAsync(Favorito favorito);
    Task RemoverAsync(int id);
}