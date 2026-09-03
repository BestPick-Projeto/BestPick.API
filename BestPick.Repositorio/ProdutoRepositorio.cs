using BestPick.Repositorio.Interfaces;
using BestPick.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IEnumerable<Produto>> BuscarPorHistoricoAsync(int historicoId)
    {
        return await contexto.Produtos
        .Where(h => h.HistoricoBuscaId == historicoId)
        .ToListAsync();
    }

    public async Task<Produto> BuscarPorIdAsync(int id)
    {
        return await contexto.Produtos
        .Where(h => h.Id == id)
        .FirstOrDefaultAsync();
    }
}