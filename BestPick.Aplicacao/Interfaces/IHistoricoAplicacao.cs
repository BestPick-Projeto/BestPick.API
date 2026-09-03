using BestPick.Aplicacao.Models;
using BestPick.Dominio.Entidades;

namespace BestPick.Aplicacao.Interfaces;

public interface IHistoricoAplicacao
{
    Task<HistoricoBusca> AdicionarAsync(HistoricoCriar historicoBusca);
    Task<IEnumerable<HistoricoRespostaDto>> ListarPorUsuarioAsync(int usuarioId);
    Task RemoverAsync(int historicoId);
    Task<HistoricoRespostaDto> ObterPorIdAsync(int id);
}