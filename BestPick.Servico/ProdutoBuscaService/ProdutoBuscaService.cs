using BestPick.Servicos.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace BestPick.Servicos;

public class ProdutoBuscaService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public ProdutoBuscaService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["SerpApi:ApiKey"];
    }

    public async Task<List<ProdutoBuscaResposta>> BuscarAsync(string produto, decimal valorCompra)
    {
        if (string.IsNullOrWhiteSpace(produto))
            throw new Exception("O produto não pode estar vazio.");

        if (valorCompra <= 0)
            throw new Exception("O valor da compra deve ser maior que zero.");

        var produtos = new List<ProdutoBuscaResposta>();

        foreach (int inicio in new[] { 0, 10, 20 })
            produtos.AddRange(await BuscarSerpApiAsync(produto, inicio));

        decimal valorMinimo = valorCompra * 0.85m;

        return RemoverDuplicados(produtos)
            .Where(p => p.Preco >= valorMinimo && p.Preco <= valorCompra)
            .OrderByDescending(p => p.Avaliacao ?? 0)
            .ThenByDescending(p => p.Avaliacoes)
            .Take(8)
            .ToList();
    }

    private async Task<List<ProdutoBuscaResposta>> BuscarSerpApiAsync(string produto, int inicio)
    {
        string url =
            $"https://serpapi.com/search.json?engine=google_shopping" +
            $"&q={Uri.EscapeDataString(produto)}&gl=br&hl=pt-br" +
            $"&start={inicio}&api_key={Uri.EscapeDataString(_apiKey)}";

        var resposta = await _httpClient.GetAsync(url);
        string corpo = await resposta.Content.ReadAsStringAsync();

        if (!resposta.IsSuccessStatusCode)
            throw new Exception($"Erro SerpApi: {corpo}");

        var resultados = JObject.Parse(corpo)["shopping_results"] as JArray;

        if (resultados == null)
            return new();

        return resultados
            .Where(item =>
                item["extracted_price"] != null &&
                !string.IsNullOrWhiteSpace(item["title"]?.ToString()))
            .Select(item => new ProdutoBuscaResposta
            {
                Id = item["product_id"]?.ToString(),
                Nome = item["title"]?.ToString(),
                Preco = item["extracted_price"]?.Value<decimal>() ?? 0,
                Loja = item["source"]?.ToString(),
                Url = item["product_link"]?.ToString(),
                ImgUrl = item["thumbnail"]?.ToString(),
                Avaliacao = item["rating"]?.Value<decimal?>(),
                Avaliacoes = item["reviews"]?.Value<int>() ?? 0
            })
            .ToList();
    }

    private List<ProdutoBuscaResposta> RemoverDuplicados(List<ProdutoBuscaResposta> produtos) => produtos
            .GroupBy(p => NormalizarNome(p.Nome))
            .Select(g => g.OrderBy(p => p.Preco).First())
            .ToList();

    private string NormalizarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return string.Empty;

        string normalizado = Regex.Replace(nome.ToLower(),
            @"[^a-z0-9áàâãéèêíïóôõöúç\s]", " ");

        return Regex.Replace(normalizado, @"\s+", " ").Trim();
    }
}