using BestPick.Api.Models;
using BestPick.Aplicacao.Interfaces;
using BestPick.Dominio.Enumeradores;
using BestPick.Repositorio.Interfaces;
using BestPick.Repositorio.Functions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BestPick.Aplicacao;

public class DashboardAplicacao : IDashboardAplicacao
{
    private readonly IHistoricoRepositorio _historicoRepositorio;
    private readonly IFavoritoRepositorio _favoritoRepositorio;
    private readonly BestPickContexto _contexto;

    public DashboardAplicacao(IHistoricoRepositorio historicoRepositorio,IFavoritoRepositorio favoritoRepositorio, BestPickContexto contexto)
    {
        _historicoRepositorio = historicoRepositorio;
        _favoritoRepositorio = favoritoRepositorio;
        _contexto = contexto;
    }

    public async Task<DashboardResposta> ObterAsync(int usuarioId)
    {
        var listaHistoricos = (await _historicoRepositorio.ListarPorUsuarioAsync(usuarioId)).ToList();
        var listaFavoritos = (await _favoritoRepositorio.ListarPorUsuarioAsync(usuarioId)).ToList();

        var totalBuscas = listaHistoricos.Count;
        var totalFavoritados = listaFavoritos.Count(f => f.Status == StatusFavorito.Favoritado);
        var totalAdquiridos = listaFavoritos.Count(f => f.Status == StatusFavorito.Adquirido);
        var total = totalBuscas + totalFavoritados + totalAdquiridos;

    

        return new DashboardResposta
        {
            TotalBuscas = totalBuscas,
            TotalFavoritados = totalFavoritados,
            TotalAdquiridos = totalAdquiridos,
            PorcentagemBuscas = await CalcularPorcentagemAsync(totalBuscas, total),
            PorcentagemFavoritados = await CalcularPorcentagemAsync(totalFavoritados, total),
            PorcentagemAdquiridos = await CalcularPorcentagemAsync(totalAdquiridos, total),
        };
    }

    private async Task<decimal> CalcularPorcentagemAsync(int quantidade, int total)
    {
        var conexao = _contexto.Database.GetDbConnection();

        if (conexao.State != System.Data.ConnectionState.Open)
            await conexao.OpenAsync();

        await using var comando = conexao.CreateCommand();
        comando.CommandText = DashboardFunctions.CalcularPorcentagem;
        comando.Parameters.Add(new SqlParameter("@Quantidade", quantidade));
        comando.Parameters.Add(new SqlParameter("@Total", total));

        return Convert.ToDecimal(await comando.ExecuteScalarAsync());
    }
}