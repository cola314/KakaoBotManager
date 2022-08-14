using KakaoBotManager.Config.Exceptions;

namespace KakaoBotManager.Config;

public class EnvironmentConfig : IEnvironmentConfig
{
    public string BACKUP_FILE { get; }
    public string ADMIN_USERNAME { get; }
    public string ADMIN_PASSWORD { get; }
    public string MESSAGE_API_KEY { get; }
    public string REDIS_SERVER { get; }
    public int REDIS_PORT { get; }
    public string WEBHOOK_SECRET { get; }

    public EnvironmentConfig(ILogger<EnvironmentConfig> logger)
    {
        logger.LogInformation("Load enviroment variables");
        try
        {
            BACKUP_FILE = GetStringEnviromentOrThrow(nameof(BACKUP_FILE));
            ADMIN_USERNAME = GetStringEnviromentOrThrow(nameof(ADMIN_USERNAME));
            ADMIN_PASSWORD = GetStringEnviromentOrThrow(nameof(ADMIN_PASSWORD));
            MESSAGE_API_KEY = GetStringEnviromentOrThrow(nameof(MESSAGE_API_KEY));
            REDIS_SERVER = GetStringEnviromentOrThrow(nameof(REDIS_SERVER));
            REDIS_PORT = GetIntEnviromentOrThrow(nameof(REDIS_PORT));
            WEBHOOK_SECRET = GetStringEnviromentOrThrow(nameof(WEBHOOK_SECRET));
        }
        catch (Exception ex)
        {
            logger.LogError("Fail to Load environment variables\n{0}", ex.Message);
            Environment.Exit(1);
        }
    }

    private string GetStringEnviromentOrThrow(string key)
    {
        return Environment.GetEnvironmentVariable(key) ?? throw new BadEnvironmentVariableException(key, "");
    }

    private int GetIntEnviromentOrThrow(string key)
    {
        string env = GetStringEnviromentOrThrow(key);
        if (int.TryParse(env, out int value))
        {
            return value;
        }
        throw new BadEnvironmentVariableException($"{key}에 대한 환경변수는 숫자여야합니다\n주어진 값 : {env}");
    }
}
