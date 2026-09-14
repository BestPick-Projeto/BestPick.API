using BestPick.Servicos.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BestPick.Servicos;

public class MercadoLivreService
{
    private readonly HttpClient _httpClient;
    private readonly string _accessToken;

    private const string API_URL =
        "https://api.mercadolibre.com";

    public MercadoLivreService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;

        _accessToken =
            configuration["MercadoLivre:AccessToken"];

        if (!string.IsNullOrWhiteSpace(_accessToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    _accessToken
                );
        }
    }

    public async Task<List<MercadoLivreProdutoResposta>> BuscarProdutosAsync(
        string produto,
        decimal valorMaximo)
    {
        if (string.IsNullOrWhiteSpace(produto))
            throw new Exception(
                "Produto não informado."
            );

        if (valorMaximo <= 0)
            throw new Exception(
                "Valor máximo inválido."
            );

        string termo =
            Uri.EscapeDataString(produto);

        string url =
            $"{API_URL}/sites/MLB/search?q={termo}&limit=30";

        HttpResponseMessage resposta =
            await _httpClient.GetAsync(url);

        string corpo =
            await resposta.Content.ReadAsStringAsync();

        if (!resposta.IsSuccessStatusCode)
        {
            throw new Exception(
                $"Erro ao consultar produtos do Mercado Livre: {corpo}"
            );
        }

        var resultado =
            JsonConvert.DeserializeObject<MercadoLivreResposta>(
                corpo
            );

        if (resultado?.Results == null ||
            resultado.Results.Count == 0)
        {
            return new List<MercadoLivreProdutoResposta>();
        }

        var produtosEncontrados =
            new List<MercadoLivreProdutoResposta>();

        foreach (var item in resultado.Results.Take(20))
        {
            try
            {
                decimal? preco =
                    await BuscarPrecoAsync(item.Id);

                if (!preco.HasValue)
                    continue;

                if (preco.Value <= 0)
                    continue;

                if (preco.Value > valorMaximo)
                    continue;

                var produtoResposta =
                    new MercadoLivreProdutoResposta
                    {
                        Id = item.Id,
                        Nome = item.Title,
                        Preco = preco.Value,
                        Url = item.Permalink,
                        ImgUrl = item.Thumbnail
                    };

                produtosEncontrados.Add(
                    produtoResposta
                );
            }
            catch
            {
                continue;
            }
        }

        return produtosEncontrados
            .OrderByDescending(p => p.Preco)
            .Take(10)
            .ToList();
    }

    private async Task<decimal?> BuscarPrecoAsync(
        string itemId)
    {
        string url =
            $"{API_URL}/items/{itemId}/prices";

        HttpResponseMessage resposta =
            await _httpClient.GetAsync(url);

        if (!resposta.IsSuccessStatusCode)
            return null;

        string corpo =
            await resposta.Content.ReadAsStringAsync();

        var resultado =
            JsonConvert.DeserializeObject<MercadoLivrePrecoResposta>(
                corpo
            );

        if (resultado?.Prices == null ||
            resultado.Prices.Count == 0)
        {
            return null;
        }

        var precosSemRestricao =
            resultado.Prices
            .Where(p =>
                p.Amount > 0 &&
                (
                    p.Conditions == null ||
                    p.Conditions.ContextRestrictions == null ||
                    p.Conditions.ContextRestrictions.Count == 0
                )
            )
            .ToList();

        if (precosSemRestricao.Count == 0)
            return null;

        var promocao =
            precosSemRestricao
            .Where(p => p.Type == "promotion")
            .OrderBy(p => p.Amount)
            .FirstOrDefault();

        if (promocao != null)
            return promocao.Amount;

        var precoNormal =
            precosSemRestricao
            .Where(p => p.Type == "standard")
            .OrderBy(p => p.Amount)
            .FirstOrDefault();

        if (precoNormal != null)
            return precoNormal.Amount;

        return precosSemRestricao
            .OrderBy(p => p.Amount)
            .First()
            .Amount;
    }
}