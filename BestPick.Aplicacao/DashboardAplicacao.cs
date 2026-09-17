using BestPick.Api.Models;
using BestPick.Aplicacao.Interfaces;
using BestPick.Dominio.Enumeradores;
using BestPick.Repositorio.Interfaces;
using System.Globalization;

namespace BestPick.Aplicacao;

public class DashboardAplicacao : IDashboardAplicacao
{
    private readonly IHistoricoRepositorio _historicoRepositorio;
    private readonly IFavoritoRepositorio _favoritoRepositorio;

    public DashboardAplicacao(IHistoricoRepositorio historicoRepositorio, IFavoritoRepositorio favoritoRepositorio)
    {
        _historicoRepositorio = historicoRepositorio;
        _favoritoRepositorio = favoritoRepositorio;
    }

    public async Task<DashboardResposta> ObterAsync(int usuarioId)
    {
        var historicos =
            await _historicoRepositorio.ListarPorUsuarioAsync(usuarioId);

        var favoritos =
            await _favoritoRepositorio.ListarPorUsuarioAsync(usuarioId);

        var listaHistoricos = historicos.ToList();
        var listaFavoritos = favoritos.ToList();

        var buscasPorMes = listaHistoricos
            .GroupBy(h => new
            {
                h.DataBusca.Year,
                h.DataBusca.Month
            })
            .OrderBy(g => g.Key.Year)
            .ThenBy(g => g.Key.Month)
            .Select(g => new DashboardBuscaMes
            {
                Mes = CultureInfo
                    .GetCultureInfo("pt-BR")
                    .DateTimeFormat
                    .GetAbbreviatedMonthName(g.Key.Month)
                    .Replace(".", ""),

                Quantidade = g.Count()
            })
            .ToList();

        return new DashboardResposta
        {
            TotalBuscas = listaHistoricos.Count,

            TotalFavoritados = listaFavoritos.Count(f =>
                f.Status == StatusFavorito.Favoritado),

            TotalAdquiridos = listaFavoritos.Count(f =>
                f.Status == StatusFavorito.Adquirido),

            BuscasPorMes = buscasPorMes
        };
    }
}