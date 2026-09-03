using BestPick.Aplicacao.Models;
using BestPick.Dominio.Entidades;
namespace BestPick.Aplicacao.Interfaces;

public interface IFavoritoAplicacao
{
    Task<Favorito> AdicionarAsync(FavoritoCriar  favorito);
    Task<FavoritoRespostaDto> ObterPorIdAsync(int id);
    Task<IEnumerable<FavoritoRespostaDto>> ListarPorUsuarioAsync(int usuarioId);
    Task AtualizarStatusAsync(FavoritoAtualizar  favorito);
    Task RemoverAsync(int id);
}