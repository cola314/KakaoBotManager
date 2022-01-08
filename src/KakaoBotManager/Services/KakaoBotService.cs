using EngineIOSharp.Common.Enum;
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
    const string SERVER_URI = "***REMOVED***";
    const int SERVER_PORT = 9200;

    private SocketIOClient client_;
    private HttpClient httpClient_;
    private readonly AddressRepository _addressRepository;
    private readonly ILogger _logger;
	public KakaoBotService(AddressRepository addressRepository, ILogger<KakaoBotService> logger)
	{
        _addressRepository = addressRepository;
        _logger = logger;
	}

    public void Run()
    {
        _logger.LogInformation("Run socket io client");
        httpClient_ = new HttpClient();
        client_ = new SocketIOClient(new SocketIOClientOption(EngineIOScheme.http, SERVER_URI, SERVER_PORT));

        var registerData = new Dictionary<string, string>()
        {
            ["password"] = "4321"
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