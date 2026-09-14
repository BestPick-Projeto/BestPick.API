using Newtonsoft.Json;

namespace BestPick.Servicos.Models;

public class ChatRequest
{
    [JsonProperty("model")]
    public string Model { get; set; }

    [JsonProperty("messages")]
    public List<Message> Messages { get; set; }

    [JsonProperty("temperature")]
    public double Temperature { get; set; }

    [JsonProperty("max_tokens")]
    public int MaxTokens { get; set; }

    [JsonProperty("compound_custom")]
    public CompoundCustom CompoundCustom { get; set; }

    [JsonProperty("search_settings")]
    public SearchSettings SearchSettings { get; set; }

    public ChatRequest(string model, List<Message> messages, double temperature, int maxTokens)
    {
        Model = model;
        Messages = messages;
        Temperature = temperature;
        MaxTokens = maxTokens;
    }
}