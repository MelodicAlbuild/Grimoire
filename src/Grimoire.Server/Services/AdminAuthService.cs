namespace Grimoire.Server.Services;

public interface IAdminAuthService
{
    bool ValidateKey(string key);
}

public class AdminAuthService : IAdminAuthService
{
    private readonly string _adminKey;

    public AdminAuthService(IConfiguration configuration)
    {
        _adminKey = configuration["AdminKey"]
            ?? throw new InvalidOperationException("AdminKey not configured in appsettings.json");
    }

    public bool ValidateKey(string key)
    {
        return !string.IsNullOrEmpty(key)
            && string.Equals(key.Trim(), _adminKey, StringComparison.Ordinal);
    }
}
