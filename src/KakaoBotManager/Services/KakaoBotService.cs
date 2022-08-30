using System.Text.Json;
using KakaoBotManager.Config;
using KakaoBotManager.Dtos;
using KakaoBotManager.Models;
using KakaoBotManager.Repository;
using StackExchange.Redis;

namespace KakaoBotManager.Services;

public class KakaoBotService
{
    private readonly string _pushChannel = "push_channel";
    private readonly string _messageQueueChannel = "message_queue";

    private readonly HttpClient _httpClient;

    private readonly IConnectionMultiplexer _redis;
    private readonly IAddressRepository _addressRepository;
    private readonly IEnvironmentConfig _config;
    private readonly ILogger _logger;

    public KakaoBotService(IAddressRepository addressRepository, ILogger<KakaoBotService> logger, IEnvironmentConfig config)
    {
        _redis = ConnectionMultiplexer.Connect($"{config.REDIS_SERVER}:{config.REDIS_PORT},abortConnect=false,ConnectTimeout=3000");
        _addressRepository = addressRepository;
        _logger = logger;
        _config = config;
        _httpClient = new HttpClient();
    }

    public async Task SendPushMessage(PushMessage message)
    {
        var json = JsonSerializer.Serialize(message);
        await _redis.GetSubscriber().PublishAsync(_pushChannel, json);
    }

    public async Task Run(CancellationToken ctx = default)
    {
        _logger.LogInformation("[Run KakaoBotService]");

        await foreach (var message in GetReceivedMessages(ctx))
        {
            SendMessages(message);
        }
    }

    private void SendMessages(ReceivedMessage message)
    {
        foreach (var address in _addressRepository.GetAll())
        {
            _logger.LogInformation("[Send message to client] {0}", address);

            var dto = new WebhookMessageDto(message, _config.WEBHOOK_SECRET);
            _httpClient.PostAsJsonAsync(address, dto)
                .ContinueWith(t =>
                {
                    _logger.LogError("[Fail to Send message to client] address: {0}, message:{1}", address, t.Exception.Message);
                }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }

    private async IAsyncEnumerable<ReceivedMessage> GetReceivedMessages(CancellationToken ctx)
    {
        while (!ctx.IsCancellationRequested)
        {
            ReceivedMessage? message = null;
            try
            {
                var value = await _redis.GetDatabase().ExecuteAsync("BRPOP", "message_queue", 4);
                if (!value.IsNull)
                {
                    var json = value.ToDictionary().FirstOrDefault().Value.ToString();
                    message = JsonSerializer.Deserialize<ReceivedMessage>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Redis Error] " + ex.Message);
            }

            if (message != null)
                yield return message;
        }
    }
}