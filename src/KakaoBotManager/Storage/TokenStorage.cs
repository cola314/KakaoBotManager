using KakaoBotManager.Repository;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace KakaoBotManager.Storage;

public interface ITokenStorage
{
    bool IsValid { get; }

    void CreateToken();
    void Expire();
    Task LoadSavedToken();
}

public class TokenStorage : ITokenStorage
{
    private const string TOKEN_KEY = "token";
    private readonly TokenRepository _tokenRepository;
    private readonly ProtectedLocalStorage _localStorage;
    public TokenStorage(TokenRepository tokenRepository, ProtectedLocalStorage localStorage)
    {
        _tokenRepository = tokenRepository;
        _localStorage = localStorage;
    }

    private Guid _userToken;

    public async Task LoadSavedToken()
    {
        _userToken = (await _localStorage.GetAsync<Guid>(TOKEN_KEY)).Value;
    }

    public void CreateToken()
    {
        _userToken = _tokenRepository.CreateToken();
        _localStorage.SetAsync(TOKEN_KEY, _userToken);
    }

    public bool IsValid => _tokenRepository.IsValidateToken(_userToken);

    public void Expire()
    {
        _tokenRepository.ExpireToken(_userToken);
    }
}
