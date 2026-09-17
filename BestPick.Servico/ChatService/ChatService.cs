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
     1000
 );

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

            if (valorCompra.HasValue && valorCompra.Value <= 0)
                valorCompra = null;

            if (string.IsNullOrWhiteSpace(produto) ||
                produto == "string")
                produto = null;

            if (string.IsNullOrWhiteSpace(motivoCompra) ||
                motivoCompra == "string")
                motivoCompra = null;

            string prompt = $@"
Você é o assistente do BestPick.

Seu objetivo é conversar com o usuário e descobrir 3 informações:

1. Qual produto ele deseja comprar.
2. Qual o valor máximo que ele pretende gastar.
3. Qual o motivo ou principal uso da compra.

Dados já identificados:

Produto: {produto ?? "não informado"}
Valor máximo: {(valorCompra.HasValue ? valorCompra.Value.ToString() : "não informado")}
Motivo: {motivoCompra ?? "não informado"}

Nova mensagem do usuário:
{mensagem}

Analise a nova mensagem e atualize os dados.

Nunca apague uma informação que já foi identificada,
a menos que o usuário claramente informe que deseja alterá-la.

Considere o valor máximo preenchido somente quando ele for maior que zero.

Se ainda faltar alguma informação,
retorne completo false e faça somente uma pergunta
para obter uma das informações que ainda estão faltando.

Se produto, valor máximo e motivo estiverem preenchidos,
retorne completo true.

Não pesquise produtos.
Não recomende produtos.
Não invente preços.
Não invente links.

Retorne somente JSON neste formato:

{{
    ""completo"": false,
    ""produto"": null,
    ""valorCompra"": null,
    ""motivoCompra"": null,
    ""mensagem"": ""pergunta ou mensagem para o usuário"",
    ""produtos"": []
}}
";

            var historico = new List<Message>
{
    new Message(
        "system",
        "Você é o assistente do BestPick. Sua função é identificar produto, orçamento máximo e motivo da compra. Retorne somente JSON válido."
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

            if (respostaBestPick.ValorCompra.HasValue &&
                respostaBestPick.ValorCompra.Value <= 0)
            {
                respostaBestPick.ValorCompra = null;
                respostaBestPick.Completo = false;
            }

            if (string.IsNullOrWhiteSpace(respostaBestPick.Produto) ||
                respostaBestPick.Produto == "string")
            {
                respostaBestPick.Produto = null;
                respostaBestPick.Completo = false;
            }

            if (string.IsNullOrWhiteSpace(respostaBestPick.MotivoCompra) ||
                respostaBestPick.MotivoCompra == "string")
            {
                respostaBestPick.MotivoCompra = null;
                respostaBestPick.Completo = false;
            }

            if (respostaBestPick.Completo)
            {
                respostaBestPick.Produtos =
                    new List<BestPickProdutoResposta>();

                respostaBestPick.Mensagem =
                    "Já tenho todas as informações necessárias para buscar as melhores opções.";
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