using KakaoBotManager.Repository;
using KakaoBotManager.Storage;

namespace KakaoBotManager.Services;

public class AddressService
{
	private readonly TokenStorage _tokenStorage;
	private readonly AddressRepository _addressRepository;

	public AddressService(TokenStorage tokenStorage, AddressRepository addressRepository)
	{
		_tokenStorage = tokenStorage;
		_addressRepository = addressRepository;
	}

	public bool IsLoaded => _addressRepository.IsLoaded;

    public void AddAddress(string url)
	{
		if (!_tokenStorage.IsValid)
			throw new UnauthorizedAccessException();

		_addressRepository.Add(url);
		_addressRepository.Save();
	}

	public void RemoveAddress(string url)
	{
		if (!_tokenStorage.IsValid)
			throw new UnauthorizedAccessException();

		_addressRepository.Remove(url);
		_addressRepository.Save();
	}

	public List<string> GetAddressList()
	{
		if (!_tokenStorage.IsValid)
			throw new UnauthorizedAccessException();

		return _addressRepository.GetAll();
	}
}
