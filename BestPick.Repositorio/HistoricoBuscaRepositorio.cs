using BestPick.Repositorio.Interfaces;
using BestPick.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BestPick.Repositorio.Repositorios;

public class HistoricoBuscaRepositorio : IHistoricoBusca
{
    private readonly BestPickContexto contexto;
    public HistoricoBuscaRepositorio(BestPickContexto bestPickContexto)
    {
        contexto = bestPickContexto;
    }
    public async Task<HistoricoBusca> AdicionarAsync(HistoricoBusca historicoBusca)
    {
        await contexto.Historicos.AddAsync(historicoBusca);
        await contexto.SaveChangesAsync();
        return historicoBusca;
    }
    public async Task<IEnumerable<HistoricoBusca>> ListarPorUsuarioAsync(int usuarioId)
    {
        return await contexto.Historicos
            .Where(h => h.UsuarioId == usuarioId)
            .OrderByDescending(h => h.DataBusca)
            .ToListAsync();
    }
    public async Task RemoverAsync(int id)
    {
        var obtido = await ObterPorIdAsync(id);
        if (obtido == null)
            throw new Exception("Historico não encontrado.");

        obtido.Ativo = false;
        await contexto.SaveChangesAsync();
    }
    public async Task<HistoricoBusca> ObterPorIdAsync(int historicoid)
    {
        return await contexto.Historicos
        .Where(f => f.Id == historicoid)
        .FirstOrDefaultAsync();
    }
}