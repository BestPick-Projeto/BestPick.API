using BestPick.Dominio.Entidades;

namespace BestPick.Repositorio.Interfaces;

public interface IHistoricoBusca
{
    Task<HistoricoBusca> AdicionarAsync(HistoricoBusca historicoBusca);
    Task<IEnumerable<HistoricoBusca>> ListarPorUsuarioAsync(int usuarioid);
    Task RemoverAsync(int id);
    Task<HistoricoBusca> ObterPorIdAsync(int id);
}