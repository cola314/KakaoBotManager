using KakaoBotManager.Config;
using KakaoBotManager.Repository;
using Moq;
using Xunit;

namespace KakaoBotManager.Test.Repository;

public class AuthRepositoryTests
{
    [Theory]
    [InlineData("id", "password", "id", "password", true)]
    [InlineData("id", "password", "x", "password", false)]
    [InlineData("id", "password", "id", "x", false)]
    [InlineData("id", "password", "x", "x", false)]
    public void IsAdmin_WhenIdAndPasswordAreSameAsConfig_ReturnTrue(
        string id, string password, string configId, string configPassword, bool areSame)
    {
        var config = new Mock<IEnvironmentConfig>();
        config.Setup(c => c.ADMIN_USERNAME).Returns(configId);
        config.Setup(c => c.ADMIN_PASSWORD).Returns(configPassword);
        var repository = new AuthRepository(config.Object);

        var result = repository.IsAdmin(id, password);

        Assert.Equal(areSame, result);
    }
}
