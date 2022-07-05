using EngineIOSharp.Common.Enum;
using KakaoBotManager.Config;
using KakaoBotManager.Repository;
using SocketIOSharp.Client;

namespace KakaoBotManager.Services;

public class Message
{
    public string Sender { get; set; }
    public string Room { get; set; }
    public string Msg { get; set; }
    public bool IsGroupChat { get; set; }
}

public class KakaoBotService : IDisposable
{
    private readonly string MESSAGE_SERVER_HOST;
    private readonly int MESSAGE_SERVER_PORT;
    private readonly string MESSAGE_API_KEY;

    private SocketIOClient client_;
    private HttpClient httpClient_;
    private readonly IAddressRepository _addressRepository;
    private readonly ILogger _logger;
	public KakaoBotService(IAddressRepository addressRepository, ILogger<KakaoBotService> logger, IEnvironmentConfig config)
	{
        MESSAGE_SERVER_HOST = config.MESSAGE_SERVER_HOST;
        MESSAGE_SERVER_PORT = config.MESSAGE_SERVER_PORT;
        MESSAGE_API_KEY = config.MESSAGE_API_KEY;

        _addressRepository = addressRepository;
        _logger = logger;

        httpClient_ = new HttpClient();
        client_ = new SocketIOClient(new SocketIOClientOption(EngineIOScheme.http, MESSAGE_SERVER_HOST, (ushort)MESSAGE_SERVER_PORT));
    }

    public void Run()
    {
        _logger.LogInformation("Run socket io client");

        var registerData = new Dictionary<string, string>()
        {
            ["password"] = MESSAGE_API_KEY
        };

        client_.On("connect", () =>
        {
            client_.Emit("register", registerData);
        });

        client_.On("receive message", (data) =>
        {
            Message message = null;
            try
			{
                message = data[0].ToObject<Message>();
            }
            catch (Exception ex)
			{
                _logger.LogError("Fail to parse message\n{0}", ex);
                return;
			}
            _logger.LogInformation("Receive message from message server");

            var domains = _addressRepository.GetAll();
            domains.AsParallel()
                .ForAll(address =>
                {
                    try
                    {
                        _logger.LogInformation("Send message to client {0}", address);
                        httpClient_.PostAsJsonAsync(address, message);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Fail to Send message to client {0}\n{1}", address, ex.Message);
                    }
                });
        });

        client_.Connect();
    }

	public void Dispose()
	{
        client_?.Dispose();
        httpClient_?.Dispose();
        client_ = null;
        httpClient_ = null;
    }
}