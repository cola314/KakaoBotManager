using System.Text.Json.Serialization;

namespace KakaoBotManager.Models;

public class ReceivedMessage
{
    [JsonPropertyName("sender")]
    public string Sender { get; init; }

    [JsonPropertyName("room")]
    public string Room { get; init; }

    [JsonPropertyName("message")]
    public string Message { get; init; }

    [JsonPropertyName("isGroupChat")]
    public bool IsGroupChat { get; init; }
}