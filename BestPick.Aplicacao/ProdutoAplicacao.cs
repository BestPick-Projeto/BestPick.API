using BestPick.Aplicacao.Interfaces;
using BestPick.Aplicacao.Models;
using BestPick.Dominio.Entidades;
using BestPick.Repositorio.Interfaces;

namespace BestPick.Aplicacao.Aplicacoes;

public class ProdutoAplicacao : IProdutoAplicacao
{
    private readonly IProdutoRepositorio _produtoRepositorio;

    public ProdutoAplicacao(IProdutoRepositorio produtoRepositorio)
    {
        _produtoRepositorio = produtoRepositorio;
    }

    private static ProdutoRespostaDto ParaDto(Produto produto)
    {
        return new ProdutoRespostaDto
        {
            Id = produto.Id,
            HistoricoBuscaId = produto.HistoricoBuscaId,
            Nome = produto.Nome,
            Categoria = produto.Categoria,
            Preco = produto.Preco,
            ImgUrl = produto.ImgUrl,
            Url = produto.Url,
            Beneficios = produto.Beneficios,
            Maleficios = produto.Maleficios
        };
    }

    public async Task<Produto> AdicionarAsync(ProdutoCriar produto)
    {
        if (produto == null)
            throw new Exception("Produto obrigatório!");

        var produtoDominio = new Produto
        {
            HistoricoBuscaId = produto.HistoricoBuscaId,
            Nome = produto.Nome,
            Categoria = produto.Categoria,
            Preco = produto.Preco,
            ImgUrl = produto.ImgUrl,
            Url = produto.Url,
            Beneficios = produto.Beneficios,
            Maleficios = produto.Maleficios
        };

        return await _produtoRepositorio.AdicionarAsync(produtoDominio);
    }

    public async Task<ProdutoRespostaDto> BuscarPorIdAsync(int id)
    {
        var obtido = await _produtoRepositorio.BuscarPorIdAsync(id);

        if (obtido == null)
            throw new Exception("Produto não encontrado!");

        return ParaDto(obtido);
    }

    public async Task<IEnumerable<ProdutoRespostaDto>> BuscarPorHistoricoAsync(int historicoBuscaId)
    {
        var produtos = await _produtoRepositorio.BuscarPorHistoricoAsync(historicoBuscaId);

        return produtos.Select(ParaDto);
    }
}