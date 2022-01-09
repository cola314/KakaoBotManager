namespace KakaoBotManager.Config;

public class DevelopmentEnvironmentConfig : IEnvironmentConfig
{
    public string ADMIN_PASSWORD => "password";

    public string ADMIN_USERNAME => "user";

    public string BACKUP_FILE => "data/backup.dat";

    public string MESSAGE_API_KEY => "";

    public string MESSAGE_SERVER_HOST => "localhost";

    public int MESSAGE_SERVER_PORT => 1234;
}
