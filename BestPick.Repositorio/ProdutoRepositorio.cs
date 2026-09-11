using BestPick.Repositorio.Interfaces;
using BestPick.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using BestPick.Repositorio.Queries;
using Microsoft.Data.SqlClient;

namespace BestPick.Repositorio.Repositorios;

public class ProdutoRepositorio : IProdutoRepositorio
{
    private readonly BestPickContexto contexto;
    public ProdutoRepositorio(BestPickContexto bestPickContexto)
    {
        contexto = bestPickContexto;
    }

    public async Task<Produto> AdicionarAsync(Produto produto)
    {
        await contexto.AddAsync(produto);
        await contexto.SaveChangesAsync();
        return produto;
    }

    public async Task<IEnumerable<Produto>> BuscarPorHistoricoAsync(int HistoricoId)
    {
        var parametro = new SqlParameter("@HistoricoId", HistoricoId);

        var obtidos = await contexto.Produtos.FromSqlRaw
        (ProdutoQueries.BuscarPorHistorico, parametro)
        .ToListAsync();

        if (obtidos == null)
            throw new Exception("Histórico não possui produtos!");

        return obtidos;
    }

    public async Task<Produto> BuscarPorIdAsync(int id)
    {
        return await contexto.Produtos
        .Where(h => h.Id == id)
        .FirstOrDefaultAsync();
    }
}