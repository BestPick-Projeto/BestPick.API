using BestPick.Dominio.Entidades;
using BestPick.Aplicacao.Models;

namespace BestPick.Aplicacao.Interfaces;

public interface IProdutoAplicacao
{
    Task<Produto> AdicionarAsync(ProdutoCriar produto);
    Task<IEnumerable<ProdutoRespostaDto>> BuscarPorHistoricoAsync(int HistoricoId);
    Task<ProdutoRespostaDto> BuscarPorIdAsync(int id);
}