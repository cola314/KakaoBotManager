namespace KakaoBotManager.Config;

public interface IEnvironmentConfig
{
    string ADMIN_PASSWORD { get; }
    string ADMIN_USERNAME { get; }
    string BACKUP_FILE { get; }
    string MESSAGE_API_KEY { get; }
    string REDIS_SERVER { get; }
    int REDIS_PORT { get; }
    string WEBHOOK_SECRET { get; }
}
