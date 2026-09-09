using BestPick.Dominio.Entidades;

namespace BestPick.Repositorio.Interfaces;

public interface IHistoricoRepositorio
{
    Task<HistoricoBusca> AdicionarAsync(HistoricoBusca historicoBusca);
    Task<IEnumerable<HistoricoBusca>> ListarPorUsuarioAsync(int usuarioid);
    Task RemoverAsync(int Id);
    Task<HistoricoBusca> ObterPorIdAsync(int historicoId, int usuarioId);
}