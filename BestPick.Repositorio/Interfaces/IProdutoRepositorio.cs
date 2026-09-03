using BestPick.Dominio.Entidades;

namespace BestPick.Repositorio.Interfaces;

public interface IProdutoRepositorio
{
    Task<Produto> AdicionarAsync(Produto produto);
    Task<IEnumerable<Produto>> BuscarPorHistoricoAsync(int HistoricoId);
    Task<Produto> BuscarPorIdAsync(int id);
}