using BestPick.Servicos.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

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
            new AuthenticationHeaderValue("Bearer", configuration["Groq:ApiKey"]);
    }

    public async Task<BestPickChatResposta> EnviarMensagemAsync(
        string mensagem, string produto = null, decimal? valorCompra = null, string motivoCompra = null)
    {
        if (string.IsNullOrWhiteSpace(mensagem))
            throw new Exception("A mensagem não pode estar vazia.");

        mensagem = mensagem.Trim();
        produto = LimparTexto(produto);
        motivoCompra = LimparTexto(motivoCompra);

        if (valorCompra <= 0)
            valorCompra = null;

        var resultado = await IdentificarDadosAsync(
            mensagem, produto, valorCompra, motivoCompra);

        if (!resultado.Completo)
            return resultado;

        var produtos = await _produtoBuscaService.BuscarAsync(
            resultado.Produto, resultado.ValorCompra.Value);

        if (produtos.Count == 0)
        {
            resultado.Produtos = new();
            resultado.Mensagem = "Não encontrei produtos dentro do orçamento informado.";
            return resultado;
        }

        resultado.Produtos = await SelecionarProdutosAsync(
            produtos, resultado.Produto, resultado.MotivoCompra, resultado.ValorCompra.Value);

        resultado.Mensagem =
            "Encontrei as opções com melhor custo-benefício para o que você procura.";

        return resultado;
    }

    private async Task<BestPickChatResposta> IdentificarDadosAsync(
    string mensagem, string produto, decimal? valorCompra, string motivoCompra)
    {
        Console.WriteLine("=== DADOS RECEBIDOS ===");
        Console.WriteLine($"Mensagem: {mensagem}");
        Console.WriteLine($"Produto: {produto}");
        Console.WriteLine($"Valor: {valorCompra}");
        Console.WriteLine($"Motivo: {motivoCompra}");

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

        string resposta = await EnviarGroqAsync(
            "Extraia os dados da compra e retorne somente JSON.", prompt);

        Console.WriteLine("=== RESPOSTA DA GROQ ===");
        Console.WriteLine(resposta);

        var resultado = JsonConvert.DeserializeObject<BestPickChatResposta>(
            LimparJson(resposta))
            ?? throw new Exception("A IA não retornou uma resposta válida.");

        resultado.Produto ??= produto;
        resultado.ValorCompra ??= valorCompra;
        resultado.MotivoCompra ??= motivoCompra;

        resultado.Completo =
            !string.IsNullOrWhiteSpace(resultado.Produto) &&
            resultado.ValorCompra > 0 &&
            !string.IsNullOrWhiteSpace(resultado.MotivoCompra);
            
        resultado.Mensagem = resultado.Completo ? "" :
            string.IsNullOrWhiteSpace(resultado.Produto) ? "Qual produto você deseja comprar?" :
            !resultado.ValorCompra.HasValue || resultado.ValorCompra <= 0 ? "Qual o valor da compra?" :
            "Qual o motivo da compra?";

        resultado.Produtos = new();

        Console.WriteLine("=== RESULTADO FINAL ===");
        Console.WriteLine($"Produto: {resultado.Produto}");
        Console.WriteLine($"Valor: {resultado.ValorCompra}");
        Console.WriteLine($"Motivo: {resultado.MotivoCompra}");
        Console.WriteLine($"Completo: {resultado.Completo}");
        Console.WriteLine($"Pergunta: {resultado.Mensagem}");

        return resultado;
    }
    private async Task<List<BestPickProdutoResposta>> SelecionarProdutosAsync(
        List<ProdutoBuscaResposta> produtos, string produto, string motivoCompra, decimal valorCompra)
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
Se não houver informações suficientes, retorne uma lista vazia de benefícios.

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
                resultado.Any(p => p.Nome == produtos[indice.Value].Nome))
                continue;

            resultado.Add(CriarProdutoResposta(
                produtos[indice.Value],
                produto,
                escolhido["beneficios"]?.ToObject<List<string>>()));

            if (resultado.Count == 3)
                return resultado;
        }

        foreach (var candidato in produtos)
        {
            if (resultado.Count == 3)
                break;

            if (!resultado.Any(p => p.Nome == candidato.Nome))
                resultado.Add(CriarProdutoResposta(candidato, produto, null));
        }

        return resultado;
    }

    private BestPickProdutoResposta CriarProdutoResposta(
        ProdutoBuscaResposta produto, string categoria, List<string> beneficios) =>
        new()
        {
            Nome = produto.Nome,
            Categoria = categoria,
            Preco = produto.Preco,
            ImgUrl = produto.ImgUrl,
            Url = produto.Url,
            Beneficios = beneficios?.Take(3).ToList() ?? new()
        };

    private async Task<string> EnviarGroqAsync(string sistema, string prompt)
    {
        var requisicao = new ChatRequest(
            _model,
            new List<Message>
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

        var resposta = await _httpClient.PostAsync(API_URL, conteudo);
        string corpo = await resposta.Content.ReadAsStringAsync();

        if (!resposta.IsSuccessStatusCode)
            throw new Exception($"Erro Groq: {corpo}");

        return JsonConvert.DeserializeObject<ChatResponse>(corpo)
            ?.Choices?[0]?.Message?.Content
            ?? throw new Exception("A Groq retornou uma resposta vazia.");
    }

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

    private string LimparTexto(string texto) =>
        string.IsNullOrWhiteSpace(texto) || texto == "string"
            ? null
            : texto;
}