using System.Text.Json.Serialization;

namespace KakaoBotManager.Models;

public class PushMessage
{
    [JsonPropertyName("room")]
    public string Room { get; init; }

    [JsonPropertyName("message")]
    public string Message { get; init; }
}