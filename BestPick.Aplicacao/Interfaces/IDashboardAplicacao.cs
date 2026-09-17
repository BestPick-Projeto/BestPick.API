using BestPick.Api.Models;

namespace BestPick.Aplicacao.Interfaces;

public interface IDashboardAplicacao
{
    Task<DashboardResposta> ObterAsync(int usuarioId);
}