using BestPick.Servicos.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace BestPick.Servicos;

public class ChatService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _model;

    private const string API_URL =
        "https://api.groq.com/openai/v1/chat/completions";

    public ChatService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;

        _apiKey = configuration["Groq:ApiKey"];
        _model = configuration["Groq:Model"];

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _apiKey);

        _httpClient.DefaultRequestHeaders.Add(
            "Groq-Model-Version",
            "latest"
        );
    }

    public async Task<string> EnviarPerguntaAsync(List<Message> historico)
    {
        try
        {
            if (historico == null || historico.Count == 0)
                throw new Exception("O histórico de mensagens está vazio.");

            var requisicao = new ChatRequest(
     _model,
     historico,
     0.1,
     3000
 );

            requisicao.SearchSettings = new SearchSettings
            {
                IncludeDomains = new List<string>
    {
        "mercadolivre.com.br",
        "amazon.com.br",
        "shopee.com.br"
    },

                Country = "brazil"
            };
            string json =
                JsonConvert.SerializeObject(requisicao);

            Console.WriteLine(json);
            var conteudo = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            HttpResponseMessage resposta =
                await _httpClient.PostAsync(
                    API_URL,
                    conteudo
                );

            string corpoResposta =
                await resposta.Content.ReadAsStringAsync();

            if (!resposta.IsSuccessStatusCode)
                return $"[ERRO GROQ]: {corpoResposta}";

            var chatResponse =
                JsonConvert.DeserializeObject<ChatResponse>(
                    corpoResposta
                );

            if (chatResponse?.Choices == null ||
                chatResponse.Choices.Count == 0 ||
                chatResponse.Choices[0]?.Message == null)
            {
                return "[Sem resposta válida da IA]";
            }

            return chatResponse.Choices[0].Message.Content
                ?? "[Sem resposta]";
        }
        catch (HttpRequestException ex)
        {
            return $"[ERRO DE CONEXÃO]: {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"[ERRO]: {ex.Message}";
        }
    }

    public async Task<BestPickChatResposta> EnviarMensagemAsync(
        string mensagem,
        string produto = null,
        decimal? valorCompra = null,
        string motivoCompra = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(mensagem))
                throw new Exception("A mensagem não pode estar vazia.");

            string prompt = $@"
Você é o assistente do BestPick.

Produto: {produto ?? "não informado"}
Valor máximo: {(valorCompra.HasValue ? valorCompra.Value.ToString() : "não informado")}
Motivo: {motivoCompra ?? "não informado"}

Mensagem:
{mensagem}

Se produto, valor ou motivo estiverem faltando,
retorne completo false e pergunte somente o dado faltante.

Se todos estiverem preenchidos:
pesquise produtos atuais nas lojas permitidas,
não invente preços ou links,
considere somente produtos dentro do orçamento
e retorne 3 opções adequadas ao motivo da compra.

Retorne somente JSON:

{{
    ""completo"": true,
    ""produto"": ""produto"",
    ""valorCompra"": 0,
    ""motivoCompra"": ""motivo"",
    ""mensagem"": ""Encontrei algumas opções para você."",
    ""produtos"": [
        {{
            ""nome"": ""nome"",
            ""categoria"": ""categoria"",
            ""preco"": 0,
            ""imgUrl"": """",
            ""url"": ""url real"",
            ""beneficios"": [],
            ""maleficios"": []
        }}
    ]
}}
";

            var historico = new List<Message>
{
    new Message(
        "system",
        "Você é o assistente do BestPick. Use as ferramentas de pesquisa quando necessário e retorne somente JSON válido."
    ),

    new Message(
        "user",
        prompt
    )
};

            string resposta =
                await EnviarPerguntaAsync(historico);

            if (string.IsNullOrWhiteSpace(resposta))
                throw new Exception(
                    "A IA retornou uma resposta vazia."
                );

            if (resposta.StartsWith("[ERRO"))
                throw new Exception(resposta);

            resposta = resposta
                .Replace("```json", "")
                .Replace("```JSON", "")
                .Replace("```", "")
                .Trim();

            var respostaBestPick =
                JsonConvert.DeserializeObject<BestPickChatResposta>(
                    resposta
                );

            if (respostaBestPick == null)
            {
                throw new Exception(
                    "A IA não retornou uma resposta válida."
                );
            }

            if (respostaBestPick.Completo)
            {
                if (respostaBestPick.Produtos == null ||
                    respostaBestPick.Produtos.Count != 3)
                {
                    throw new Exception(
                        "A IA deve retornar exatamente 3 produtos."
                    );
                }

                foreach (var produtoResposta in respostaBestPick.Produtos)
                {
                    if (string.IsNullOrWhiteSpace(produtoResposta.Nome))
                    {
                        throw new Exception(
                            "A IA retornou um produto sem nome."
                        );
                    }

                    if (produtoResposta.Preco <= 0)
                    {
                        throw new Exception(
                            "A IA retornou um produto com preço inválido."
                        );
                    }

                    if (string.IsNullOrWhiteSpace(produtoResposta.Url))
                    {
                        throw new Exception(
                            "A IA retornou um produto sem link da oferta."
                        );
                    }
                }
            }

            return respostaBestPick;
        }
        catch (JsonException ex)
        {
            throw new Exception(
                $"A IA retornou um JSON inválido: {ex.Message}"
            );
        }
        catch (Exception ex)
        {
            throw new Exception(
                $"Erro ao gerar recomendação da IA: {ex.Message}"
            );
        }
    }
}