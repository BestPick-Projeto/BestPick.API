using BestPick.Aplicacao.Models;
using BestPick.Dominio.Entidades;

namespace BestPick.Aplicacao.Interfaces;

public interface IFavoritoAplicacao
{
    Task<Favorito> AdicionarAsync(FavoritoCriar favorito, int usuarioId);
    Task<FavoritoRespostaDto> ObterPorIdAsync(int id, int usuarioId);
    Task<IEnumerable<FavoritoRespostaDto>> ListarPorUsuarioAsync(int usuarioId);
    Task AtualizarStatusAsync(FavoritoAtualizar favorito, int usuarioId);
    Task RemoverAsync(int id, int usuarioId);
}