using BestPick.Servicos.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace BestPick.Servicos;

public class ChatService
{
    private readonly HttpClient _httpClient;
    private readonly ProdutoBuscaService _produtoBuscaService;
    private readonly string _model;

    private const string API_URL =
        "https://api.groq.com/openai/v1/chat/completions";

    public ChatService(HttpClient httpClient, IConfiguration configuration, ProdutoBuscaService produtoBuscaService)
    {
        _httpClient = httpClient;
        _produtoBuscaService = produtoBuscaService;
        _model = configuration["Groq:Model"];

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                configuration["Groq:ApiKey"]);
    }

    public async Task<BestPickChatResposta> EnviarMensagemAsync(string mensagem, string produto = null, decimal? valorCompra = null, string motivoCompra = null)
    {
        if (string.IsNullOrWhiteSpace(mensagem))
            throw new Exception("A mensagem não pode estar vazia.");

        mensagem = mensagem.Trim();
        produto = LimparTexto(produto);
        motivoCompra = LimparTexto(motivoCompra);

        if (valorCompra <= 0)
            valorCompra = null; 
        BestPickChatResposta resultado;

        if (produto != null && valorCompra.HasValue && motivoCompra == null)
        {
            resultado = new()
            {
                Produto = produto,
                ValorCompra = valorCompra,
                MotivoCompra = mensagem,
                Produtos = new()
            };
        }
        else
        {
            resultado = await IdentificarDadosAsync(mensagem, produto, valorCompra, motivoCompra);

            resultado.Produto ??= produto;
            resultado.ValorCompra ??= valorCompra;
            resultado.MotivoCompra ??= motivoCompra;
        }

        if (!DadosCompletos(resultado))
        {
            resultado.Completo = false;
            resultado.Produtos = new();
            resultado.Mensagem = CriarPergunta(resultado);

            return resultado;
        }

        resultado.Completo = true;

        var produtos = await _produtoBuscaService.BuscarAsync(resultado.Produto, resultado.ValorCompra.Value);

        if (produtos.Count == 0)
        {
            resultado.Produtos = new();
            resultado.Mensagem =
                "Não encontrei produtos dentro do orçamento informado.";

            return resultado;
        }

        resultado.Produtos = await SelecionarProdutosAsync(produtos, resultado.Produto, resultado.MotivoCompra, resultado.ValorCompra.Value);

        resultado.Mensagem =
            "Encontrei as opções com melhor custo-benefício para o que você procura.";

        return resultado;
    }

    private async Task<BestPickChatResposta> IdentificarDadosAsync(string mensagem, string produto, decimal? valorCompra, string motivoCompra)
    {
        string prompt = $@"
Dados atuais:
Produto: {produto ?? "não informado"}
Valor: {(valorCompra.HasValue ? valorCompra.Value : "não informado")}
Motivo: {motivoCompra ?? "não informado"}

Nova mensagem: ""{mensagem}""

Identifique produto, valorCompra e motivoCompra.
Preserve os dados já informados.
Use a nova mensagem para preencher ou alterar dados.

Retorne SOMENTE JSON:
{{
    ""completo"": false,
    ""produto"": null,
    ""valorCompra"": null,
    ""motivoCompra"": null,
    ""mensagem"": """",
    ""produtos"": []
}}";

        string resposta = await EnviarGroqAsync("Extraia os dados da compra e retorne somente JSON.",prompt);

        return JsonConvert.DeserializeObject<BestPickChatResposta>(LimparJson(resposta))
        ?? throw new Exception(
                "A IA não retornou uma resposta válida.");
    }

    private async Task<List<BestPickProdutoResposta>> SelecionarProdutosAsync(List<ProdutoBuscaResposta> produtos, string produto, string motivoCompra, decimal valorCompra)
    {
        var candidatos = produtos.Select((p, i) => new
        {
            indice = i,
            p.Nome,
            p.Preco,
            p.Loja,
            p.Avaliacao,
            p.Avaliacoes
        });

        string prompt = $@"
Produto: {produto}
Orçamento: R$ {valorCompra}
Motivo: {motivoCompra}

Candidatos:
{JsonConvert.SerializeObject(candidatos)}

Escolha até 3 produtos diferentes.
Se houver pelo menos 3 candidatos, escolha 3.

Considere motivo, características do nome, preço,
avaliação e quantidade de avaliações juntas.

Use SOMENTE os dados fornecidos.
Não invente características ou desempenho.

Gere até 3 benefícios em português usando SOMENTE
informações presentes no nome do produto.
Não use preço, loja ou avaliações como benefício.

Retorne SOMENTE JSON:
[
    {{
        ""indice"": 0,
        ""beneficios"": [""benefício em português""]
    }}
]";

        string resposta = await EnviarGroqAsync(
            "Escolha os melhores produtos usando somente os dados fornecidos. Retorne somente JSON.",
            prompt);

        var escolhidos = JArray.Parse(ExtrairArray(resposta));

        var resultado = new List<BestPickProdutoResposta>();

        foreach (var escolhido in escolhidos)
        {
            int? indice = escolhido["indice"]?.Value<int?>();

            if (!indice.HasValue ||
                indice < 0 ||
                indice >= produtos.Count ||
                resultado.Any(p =>
                    p.Nome == produtos[indice.Value].Nome))
                continue;

            resultado.Add(CriarProdutoResposta(produtos[indice.Value], produto, escolhido["beneficios"]
                    ?.ToObject<List<string>>()));

            if (resultado.Count == 3)
                return resultado;
        }

        foreach (var candidato in produtos)
        {
            if (resultado.Count == 3)
                break;

            if (!resultado.Any(p =>
                p.Nome == candidato.Nome))
                resultado.Add(CriarProdutoResposta(candidato, produto, null));
        }

        return resultado;
    }

    private BestPickProdutoResposta CriarProdutoResposta(ProdutoBuscaResposta produto, string categoria,List<string> beneficios) =>
        new()
        {
            Nome = produto.Nome,
            Categoria = categoria,
            Preco = produto.Preco,
            ImgUrl = produto.ImgUrl,
            Url = produto.Url,
            Beneficios = beneficios?.Count > 0
                ? beneficios.Take(3).ToList()
                : CriarBeneficios(produto.Nome)
        };

    private List<string> CriarBeneficios(string nome)
    {
        var beneficios = new List<string>();

        var memoria = Regex.Match(nome ?? "",
            @"(\d+)\s*GB\s*(GDDR\d+)?",
            RegexOptions.IgnoreCase);

        var modelo = Regex.Match( nome ?? "",
            @"\b(RTX|GTX|RX)\s*\d+[A-Za-z0-9]*\b",
            RegexOptions.IgnoreCase);

        if (memoria.Success)
        {
            string quantidade = memoria.Groups[1].Value;
            string tipo = memoria.Groups[2].Value;

            beneficios.Add(string.IsNullOrWhiteSpace(tipo)
                    ? $"Possui {quantidade}GB de memória."
                    : $"Possui {quantidade}GB de memória {tipo.ToUpper()}.");
        }

        if (modelo.Success)beneficios.Add($"Pertence à linha {modelo.Value.ToUpper()}.");

        if (Regex.IsMatch(nome ?? "", @"\bOC\b", RegexOptions.IgnoreCase))
            beneficios.Add("O modelo possui identificação OC.");

        return beneficios.Take(3).ToList();
    }

    private async Task<string> EnviarGroqAsync(string sistema, string prompt)
    {
        var requisicao = new ChatRequest(_model, new List<Message>
            {
                new("system", sistema),
                new("user", prompt)
            },
            0.1,
            2000);

        using var conteudo = new StringContent(
            JsonConvert.SerializeObject(requisicao),
            Encoding.UTF8,
            "application/json");

        var resposta =
            await _httpClient.PostAsync(API_URL, conteudo);

        string corpo = await resposta.Content.ReadAsStringAsync();

        if (!resposta.IsSuccessStatusCode)
            throw new Exception($"Erro Groq: {corpo}");

        return JsonConvert
            .DeserializeObject<ChatResponse>(corpo)
            ?.Choices?[0]?.Message?.Content
            ?? throw new Exception("A Groq retornou uma resposta vazia.");
    }

    private bool DadosCompletos(BestPickChatResposta resultado) =>
    !string.IsNullOrWhiteSpace(resultado.Produto) &&
        !string.IsNullOrWhiteSpace(resultado.MotivoCompra) &&
        resultado.ValorCompra > 0;

    private string CriarPergunta(BestPickChatResposta resultado) =>
    string.IsNullOrWhiteSpace(resultado.Produto)
            ? "Qual produto você deseja comprar?"
            : !resultado.ValorCompra.HasValue ||
              resultado.ValorCompra <= 0
                ? "Qual o valor da compra?"
                : "Qual o motivo da compra?";

    private string LimparJson(string json) => json
            .Replace("```json", "")
            .Replace("```JSON", "")
            .Replace("```", "")
            .Trim();

    private string ExtrairArray(string resposta)
    {
        resposta = LimparJson(resposta);

        int inicio = resposta.IndexOf('[');
        int fim = resposta.LastIndexOf(']');

        if (inicio < 0 || fim < inicio)
            throw new Exception($"Resposta inválida da Groq: {resposta}");

        return resposta.Substring(inicio, fim - inicio + 1);
    }

    private string LimparTexto(string texto) => string.IsNullOrWhiteSpace(texto) ||
        texto == "string"
            ? null
            : texto;
}