namespace KakaoBotManager.Config;

public class DevelopmentEnvironmentConfig : IEnvironmentConfig
{
    public string ADMIN_PASSWORD => "password";

    public string ADMIN_USERNAME => "user";

    public string BACKUP_FILE => "data/backup.dat";

    public string MESSAGE_API_KEY => "ApiKey";

    public string REDIS_SERVER => "localhost";

    public int REDIS_PORT => 6379;

    public string WEBHOOK_SECRET => "secret";
}
