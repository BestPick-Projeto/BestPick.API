using BestPick.Aplicacao.Interfaces;
using BestPick.Repositorio.Interfaces;
using BestPick.Dominio.Entidades;
using BestPick.Repositorio.Repositorios;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BestPick.Aplicacao.Aplicacoes;

public class HistoricoAplicacao : IHistoricoAplicacao
{
    private readonly IHistoricoBusca _historicoRepositorio;
    public HistoricoAplicacao(IHistoricoBusca historicoAplicacao)
    {
        _historicoRepositorio = historicoAplicacao;
    }

    public async Task<HistoricoBusca> AdicionarAsync(HistoricoBusca historicoBusca)
    {
        if (historicoBusca == null)
            throw new Exception("O historico não pode ser vazio!");


        return await _historicoRepositorio.AdicionarAsync(historicoBusca);

    }
    public async Task<IEnumerable<HistoricoBusca>> ListarPorUsuarioAsync(int usuarioId)
    {
        var obtidos = await _historicoRepositorio.ListarPorUsuarioAsync(usuarioId);

        if (obtidos == null)
            throw new Exception("Usuário não possui historico!");

        return obtidos;
    }

    public async Task<HistoricoBusca> ObterPorIdAsync(int historicoId)
    {
        var obtido = await _historicoRepositorio.ObterPorIdAsync(historicoId);
        if (obtido == null)
            throw new Exception("Historico não existe!");

        return obtido;
    }
    public async Task RemoverAsync(int historicoId)
    {
        var obtido = await _historicoRepositorio.ObterPorIdAsync(historicoId);

        if (obtido == null)
            throw new Exception("Usuário não possui historico!");

        await _historicoRepositorio.RemoverAsync(obtido.Id);
    }
}