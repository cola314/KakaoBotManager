using KakaoBotManager.Exceptions;
using KakaoBotManager.Repository;
using KakaoBotManager.Storage;

namespace KakaoBotManager.Services;

public class AddressService
{
	private readonly ITokenStorage _tokenStorage;
	private readonly IAddressRepository _addressRepository;

	public AddressService(ITokenStorage tokenStorage, IAddressRepository addressRepository)
	{
		_tokenStorage = tokenStorage;
		_addressRepository = addressRepository;
	}

	public bool IsLoaded => _addressRepository.IsLoaded;

    public void AddAddress(string url)
	{
		if (!_tokenStorage.IsValid)
			throw new UnauthorizedException();

		_addressRepository.Add(url);
		_addressRepository.Save();
	}

	public void RemoveAddress(string url)
	{
		if (!_tokenStorage.IsValid)
			throw new UnauthorizedException();

		_addressRepository.Remove(url);
		_addressRepository.Save();
	}

	public List<string> GetAddressList()
	{
		if (!_tokenStorage.IsValid)
			throw new UnauthorizedException();

		return _addressRepository.GetAll();
	}
}
