using BestPick.Dominio.Entidades;

namespace BestPick.Aplicacao.Interfaces;

public interface IHistoricoAplicacao
{
    Task<HistoricoBusca> AdicionarAsync(HistoricoBusca historicoBusca);
    Task<IEnumerable<HistoricoBusca>> ListarPorUsuarioAsync(int usuarioId);
    Task RemoverAsync(int historicoId);
    Task<HistoricoBusca> ObterPorIdAsync(int id);
}