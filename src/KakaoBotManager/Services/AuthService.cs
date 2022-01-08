using KakaoBotManager.Repository;
using KakaoBotManager.Storage;

namespace KakaoBotManager.Services;

public class AuthService
{
	private readonly AuthRepository _authRepository;
	private readonly TokenStorage _tokenStorage;

	public AuthService(AuthRepository authRepository, TokenStorage tokenStorage)
	{
		_authRepository = authRepository;
		_tokenStorage = tokenStorage;
	}

    public bool TryLogin(string username, string password)
	{
		if (_authRepository.IsAdmin(username, password))
		{
			_tokenStorage.CreateToken();
			return true;
		}
		return false;
	}

	public void Logout()
	{
		_tokenStorage.Expire();
	}
}
