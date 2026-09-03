using BestPick.Aplicacao.Interfaces;
using BestPick.Repositorio.Interfaces;
using BestPick.Dominio.Entidades;
using BestPick.Aplicacao.Models;
using BestPick.Repositorio.Repositorios;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Net.Sockets;

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
    public async Task<HistoricoBusca> AdicionarAsync(HistoricoCriar historicoBusca)
    {
        if (historicoBusca == null)
            throw new Exception("O historico não pode ser vazio!");

        var historicoDominio = new HistoricoBusca
        {
            ValorCompra = historicoBusca.ValorCompra,
            MotivoCompra = historicoBusca.MotivoCompra,
            ProdutoNome = historicoBusca.ProdutoNome
        };
        return await _historicoRepositorio.AdicionarAsync(historicoDominio);
    }
    public async Task<IEnumerable<HistoricoRespostaDto>> ListarPorUsuarioAsync(int usuarioId)
    {
        var obtidos = await _historicoRepositorio.ListarPorUsuarioAsync(usuarioId);

        if (obtidos == null)
            throw new Exception("Usuário não possui historico!");

        return obtidos.Select(ParaDto);
    }

    public async Task<HistoricoRespostaDto> ObterPorIdAsync(int historicoId)
    {
        var obtido = await _historicoRepositorio.ObterPorIdAsync(historicoId);
        if (obtido == null)
            throw new Exception("Historico não existe!");

        return ParaDto(obtido);
    }
    public async Task RemoverAsync(int historicoId)
    {
        var obtido = await _historicoRepositorio.ObterPorIdAsync(historicoId);

        if (obtido == null)
            throw new Exception("Histórico não encontrado.");

        await _historicoRepositorio.RemoverAsync(obtido.Id);
    }
}