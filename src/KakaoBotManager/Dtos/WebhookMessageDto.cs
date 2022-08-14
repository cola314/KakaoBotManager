using System.Text.Json.Serialization;
using KakaoBotManager.Models;

namespace KakaoBotManager.Dtos;

public class WebhookMessageDto
{
    public WebhookMessageDto(ReceivedMessage message, string secret)
    {
        Sender = message.Sender;
        Room = message.Room;
        Message = message.Message;
        IsGroupChat = message.IsGroupChat;
        Secret = secret;
    }

    [JsonPropertyName("sender")]
    public string Sender { get; }

    [JsonPropertyName("room")]
    public string Room { get; }

    [JsonPropertyName("message")]
    public string Message { get; }

    [JsonPropertyName("isGroupChat")]
    public bool IsGroupChat { get; }

    [JsonPropertyName("secret")]
    public string Secret { get; set; }
}