using BestPick.Dominio.Entidades;
using BestPick.Dominio.Enumeradores;
namespace BestPick.Repositorio.Interfaces;

public interface IFavoritoRepositorio
{
    Task<Favorito> AdicionarAsync(Favorito favorito);
    Task<Favorito> ObterPorIdAsync(int id, int usuarioId);
    Task<IEnumerable<Favorito>> ListarPorUsuarioAsync(int usuarioId);
    Task AtualizarStatusAsync(Favorito favorito);
    Task RemoverAsync(int id);
}