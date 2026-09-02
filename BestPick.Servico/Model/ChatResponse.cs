using Newtonsoft.Json;

namespace BestPick.Servicos.Models;

public class ChatResponse
{
    [JsonProperty("choices")]
    public List<Choice> Choices { get; set; }
}

public class Choice
{
    [JsonProperty("message")]
    public Message Message { get; set; }
}