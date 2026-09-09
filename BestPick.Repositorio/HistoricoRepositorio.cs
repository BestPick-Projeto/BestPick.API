using BestPick.Repositorio.Interfaces;
using BestPick.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BestPick.Repositorio.Repositorios;

public class HistoricoRepositorio : IHistoricoRepositorio
{
    private readonly BestPickContexto contexto;
    public HistoricoRepositorio(BestPickContexto bestPickContexto)
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
            .Where(h => h.Ativo == true)
            .OrderByDescending(h => h.DataBusca)
            .ToListAsync();
    }
    public async Task<HistoricoBusca> ObterPorIdAsync(int id, int usuarioId)
    {
        return await contexto.Historicos
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.UsuarioId == usuarioId &&
                x.Ativo);
    }
    public async Task RemoverAsync(int id)
    {
        var obtido = await contexto.Historicos.FirstOrDefaultAsync(x => x.Id == id && x.Ativo);

        if (obtido == null)
            throw new Exception("Historico não encontrado.");

        obtido.Ativo = false;

        await contexto.SaveChangesAsync();
    }
}