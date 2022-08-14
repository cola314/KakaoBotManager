using KakaoBotManager.Dtos;
using KakaoBotManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace KakaoBotManager.Controllers;

[ApiController]
public class MessageController : ControllerBase
{
    private readonly KakaoBotService _kakaoBotService;
    private readonly ApiKeyService _apiKeyService;

    public MessageController(KakaoBotService kakaoBotService, ApiKeyService apiKeyService)
    {
        _kakaoBotService = kakaoBotService;
        _apiKeyService = apiKeyService;
    }

    [HttpPost("api/message/push")]
    public async Task<IActionResult> SendMessage(PushMessageDto message)
    {
        if (!_apiKeyService.IsValidApiKey(message.ApiKey))
            return Unauthorized("Invalid ApiKey");

        await _kakaoBotService.SendPushMessage(message.ToPushMessage());

        return Ok();
    }
}