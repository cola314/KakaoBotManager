using KakaoBotManager.Repository;
using KakaoBotManager.Services;
using KakaoBotManager.Storage;
using Moq;
using System;
using KakaoBotManager.Exceptions;
using Xunit;

namespace KakaoBotManager.Test.Services;

public class AddressServiceTests
{
    private Mock<ITokenStorage> _storage;
    private Mock<IAddressRepository> _repository;
    private AddressService _service;

    public AddressServiceTests()
    {
        _storage = new Mock<ITokenStorage>();
        _repository = new Mock<IAddressRepository>();
        _service = new AddressService(_storage.Object, _repository.Object);
    }

    [Fact]
    public void AddAddress_WhenTokenIsInvalid_ThrowsUnauthorizedException()
    {
        _storage.Setup(s => s.IsValid).Returns(false);

        Assert.Throws<UnauthorizedException>(() => _service.AddAddress("url"));
    }

    [Fact]
    public void AddAddress_WhenTokenIsValid_AddAddress()
    {
        _storage.Setup(s => s.IsValid).Returns(true);

        _service.AddAddress("url");

        _repository.Verify(r => r.Add("url"));
        _repository.Verify(r => r.Save());
    }

    [Fact]
    public void RemoveAddress_WhenTokenIsInvalid_ThrowsUnauthorizedException()
    {
        _storage.Setup(s => s.IsValid).Returns(false);

        Assert.Throws<UnauthorizedException>(() => _service.RemoveAddress("url"));
    }

    [Fact]
    public void RemoveAddress_WhenTokenIsValid_AddAddress()
    {
        _storage.Setup(s => s.IsValid).Returns(true);

        _service.RemoveAddress("url");

        _repository.Verify(r => r.Remove("url"));
        _repository.Verify(r => r.Save());
    }

    [Fact]
    public void IsLoaded_WhenAddressRepositoryIsLoaded_ReturnTrue()
    {
        _repository.Setup(r => r.IsLoaded).Returns(true);

        var result = _service.IsLoaded;

        Assert.True(result);
    }

    [Fact]
    public void IsLoaded_WhenAddressRepositoryIsNotLoaded_ReturnFalse()
    {
        _repository.Setup(r => r.IsLoaded).Returns(false);

        var result = _service.IsLoaded;

        Assert.False(result);
    }

    [Fact]
    public void GetAddressList_WhenTokenIsInvalid_ThrowsUnauthorizedException()
    {
        _storage.Setup(s => s.IsValid).Returns(false);

        Assert.Throws<UnauthorizedException>(() => _service.GetAddressList());
    }

    [Fact]
    public void GetAddressList_WhenTokenIsValidAndAddTwoUrls_ReturnAddedTwoUrls()
    {
        _storage.Setup(s => s.IsValid).Returns(true);

        _service.GetAddressList();

        _repository.Verify(r => r.GetAll());
    }
}
