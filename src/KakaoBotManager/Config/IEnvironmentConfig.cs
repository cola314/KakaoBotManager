namespace KakaoBotManager.Config;

public interface IEnvironmentConfig
{
    string ADMIN_PASSWORD { get; }
    string ADMIN_USERNAME { get; }
    string BACKUP_FILE { get; }
    string MESSAGE_API_KEY { get; }
    string MESSAGE_SERVER_HOST { get; }
    int MESSAGE_SERVER_PORT { get; }
}
