using System.Runtime.CompilerServices;
using System.Text.Json;

namespace KakaoBotManager.Repository;

public class AddressRepository
{
    private readonly string BACKUP_FILE = Path.Join(Directory.GetCurrentDirectory(), "/data", "backup.dat");

	private Lazy<List<string>> _addresses;

	private readonly ILogger<AddressRepository> _logger;

	public bool IsLoaded { get; private set; }

	public AddressRepository(ILogger<AddressRepository> logger)
	{
		_logger = logger;
		_addresses = new Lazy<List<string>>(() =>
		{
			IsLoaded = true;
			try  
			{
				var dir = Path.GetDirectoryName(BACKUP_FILE);
				if (!Directory.Exists(dir))
					Directory.CreateDirectory(dir);

				if (!File.Exists(BACKUP_FILE))
					return new List<string>();

				_logger.LogInformation("Load addresses files");
				return JsonSerializer.Deserialize<List<string>>(File.ReadAllText(BACKUP_FILE));
			}
			catch (Exception ex)
			{
				IsLoaded = false;
				_logger.LogError("Fail to load addresses files {0}", ex.Message);
			}
			return new List<string>();
		});
	}

	public void Add(string url)
	{
		_addresses.Value.Add(url);
	}

	public void Remove(string url)
	{
		_addresses.Value.Remove(url);
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public void Save()
	{
		try
		{
			_logger.LogInformation("Save addresses to file {0}", BACKUP_FILE);
			File.WriteAllText(BACKUP_FILE, JsonSerializer.Serialize(_addresses.Value));
		}
		catch (Exception ex)
		{
			_logger.LogError("Fail to save addresses to file {0}", ex.Message);
		}
	}

	public List<string> GetAll()
	{
		return _addresses.Value;
	}
}
