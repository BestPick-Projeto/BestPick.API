using BestPick.Aplicacao.Interfaces;
using BestPick.Repositorio.Interfaces;
using BestPick.Dominio.Entidades;
using BestPick.Aplicacao.Models;

namespace BestPick.Aplicacao.Aplicacoes;

public class HistoricoAplicacao : IHistoricoAplicacao
{
    private readonly IHistoricoRepositorio _historicoRepositorio;
    public HistoricoAplicacao(IHistoricoRepositorio historicorepositorio)
    {
        _historicoRepositorio = historicorepositorio;
    }

    private static HistoricoRespostaDto ParaDto(HistoricoBusca historicoBusca)
    {
        return new HistoricoRespostaDto
        {
            Id = historicoBusca.Id,
            ProdutoNome = historicoBusca.ProdutoNome,
            MotivoCompra = historicoBusca.MotivoCompra,
            ValorCompra = historicoBusca.ValorCompra,
            DataBusca = historicoBusca.DataBusca
        };
    }
    public async Task<HistoricoBusca> AdicionarAsync(HistoricoCriar historicoBusca, int usuarioId)
    {
        if (historicoBusca == null)
            throw new Exception("O historico não pode ser vazio!");

        var historicoDominio = new HistoricoBusca
        {
            UsuarioId = usuarioId,
            ValorCompra = historicoBusca.ValorCompra,
            MotivoCompra = historicoBusca.MotivoCompra,
            ProdutoNome = historicoBusca.ProdutoNome,
            DataBusca = DateTime.Now
        };
        return await _historicoRepositorio.AdicionarAsync(historicoDominio);
    }
    public async Task<IEnumerable<HistoricoRespostaDto>> ListarPorUsuarioAsync(int usuarioId)
    {
        var obtidos = await _historicoRepositorio.ListarPorUsuarioAsync(usuarioId);

        return obtidos.Select(ParaDto);
    }

    public async Task<HistoricoRespostaDto> ObterPorIdAsync(int id, int usuarioId)
    {
        var obtido = await _historicoRepositorio.ObterPorIdAsync(id, usuarioId);

        if (obtido == null)
            throw new Exception("Histórico não encontrado!");

        return new HistoricoRespostaDto
        {
            Id = obtido.Id,
            ProdutoNome = obtido.ProdutoNome,
            MotivoCompra = obtido.MotivoCompra,
            ValorCompra = obtido.ValorCompra,
            DataBusca = obtido.DataBusca
        };
    }
    public async Task RemoverAsync(int historicoId, int usuarioId)
    {
        var obtido = await _historicoRepositorio.ObterPorIdAsync(historicoId, usuarioId);

        if (obtido == null)
            throw new Exception("Histórico não encontrado!");

        await _historicoRepositorio.RemoverAsync(obtido.Id);
    }
}