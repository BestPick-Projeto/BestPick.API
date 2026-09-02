using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using BestPick.Servicos.Models;

namespace BestPick.Servicos.ChatService;

public class ChatService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _model;

    private const string API_URL = "https://api.groq.com/openai/v1/chat/completions";

    public ChatService(string apiKey, string model = "llama-3.3-70b-versatile")
    {
        _apiKey = apiKey;
        _model = model;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<string> EnviarPerguntaAsync(List<Message> historico)
    {
        try
        {
            var requisicao = new ChatRequest(_model, historico, 0.7, 1000);
            string json = JsonConvert.SerializeObject(requisicao);
            var conteudo = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage resposta = await _httpClient.PostAsync(API_URL, conteudo);

            string corpoResposta = await resposta.Content.ReadAsStringAsync();
            var chatResponse = JsonConvert.DeserializeObject<ChatResponse>(corpoResposta);
            return chatResponse?.Choices[0]?.Message?.Content ?? "[Sem resposta]";
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
}