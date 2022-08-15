using KakaoBotManager.Config;

namespace KakaoBotManager.Services;

public class ApiKeyService
{
    private readonly IEnvironmentConfig _config;

    public ApiKeyService(IEnvironmentConfig config)
    {
        _config = config;
    }

    public bool IsValidApiKey(string apiKey)
    {
        return _config.MESSAGE_API_KEY == apiKey;
    }
}