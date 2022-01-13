using KakaoBotManager.Repository;
using System;
using Xunit;

namespace KakaoBotManager.Test.Repository;

public class TokenRepositoryTests
{
    private TokenRepository _repository;

    public TokenRepositoryTests()
    {
        _repository = new TokenRepository();
    }

    [Fact]
    public void CreateToken_WhenCalled_ReturnsNotEmptyToken()
    {
        var result = _repository.CreateToken();

        Assert.True(Guid.Empty != result);
    }

    [Fact]
    public void IsValidateToken_TokenIsLatestCreatedOne_ReturnsTrue()
    {
        var token = _repository.CreateToken();

        var result = _repository.IsValidateToken(token);

        Assert.True(result);
    }

    [Fact]
    public void IsValidateToken_AfterOtherTokenIsCreated_ReturnsFalse()
    {
        var token = _repository.CreateToken();
        _repository.CreateToken();

        var result = _repository.IsValidateToken(token);

        Assert.False(result);
    }

    [Fact]
    public void IsValidateToken_InputIsExpiredToken_ReturnsFalse()
    {
        var token = _repository.CreateToken();
        _repository.ExpireToken(token);

        var result = _repository.IsValidateToken(token);

        Assert.False(result);
    }
}
