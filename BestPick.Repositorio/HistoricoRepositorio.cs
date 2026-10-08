using BestPick.Repositorio.Interfaces;
using BestPick.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using BestPick.Repositorio.Views;
using Microsoft.Data.SqlClient;

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
        var parametro = new SqlParameter("@UsuarioId", usuarioId);

        var obtidos = await contexto.Historicos.FromSqlRaw
        (HistoricoView.ListarPorUsuario, parametro)
        .ToListAsync();

        if (obtidos == null)
            throw new Exception("Usuário não possui históricos!");

        return obtidos;
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