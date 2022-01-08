namespace KakaoBotManager.Repository;

public class TokenRepository
{
	private Guid? uniqueToken;

    public Guid CreateToken()
	{
		uniqueToken = Guid.NewGuid();
		return uniqueToken.Value;
	}

	public bool IsValidateToken(Guid guid)
	{
		return guid == uniqueToken;
	}

	public void ExpireToken(Guid _userToken)
	{
		if (uniqueToken == _userToken)
		{
			uniqueToken = null;
		}
	}
}
