using BestPick.Aplicacao.Models;
using BestPick.Dominio.Entidades;

namespace BestPick.Aplicacao.Interfaces;

public interface IHistoricoAplicacao
{
    Task<HistoricoBusca> AdicionarAsync(HistoricoCriar historicoBusca, int usuarioId);
    Task<IEnumerable<HistoricoRespostaDto>> ListarPorUsuarioAsync(int usuarioId);
    Task RemoverAsync(int historicoId, int usuarioId);
    Task<HistoricoRespostaDto> ObterPorIdAsync(int id, int usuarioId);
}