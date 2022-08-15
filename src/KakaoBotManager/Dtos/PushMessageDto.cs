using System.Text.Json.Serialization;
using KakaoBotManager.Models;

namespace KakaoBotManager.Dtos;

public class PushMessageDto
{
    [JsonPropertyName("room")]
    public string Room { get; init; }

    [JsonPropertyName("message")]
    public string Message { get; init; }

    [JsonPropertyName("apiKey")] 
    public string ApiKey { get; init; }

    public PushMessage ToPushMessage()
    {
        return new PushMessage()
        {
            Room = Room,
            Message = Message,
        };
    }
}