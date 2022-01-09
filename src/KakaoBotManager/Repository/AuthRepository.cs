using KakaoBotManager.Config;

namespace KakaoBotManager.Repository;

public class AuthRepository
{
    private readonly string ADMIN_USERNAME;
    private readonly string ADMIN_PASSWORD;

    public AuthRepository(IEnvironmentConfig config)
    {
        ADMIN_USERNAME = config.ADMIN_USERNAME;
        ADMIN_PASSWORD = config.ADMIN_PASSWORD;
    }

    public bool IsAdmin(string username, string password)
	{
        return username == ADMIN_USERNAME && password == ADMIN_PASSWORD;
	}
}
