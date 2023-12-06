using Microsoft.Extensions.Configuration;

namespace ExchangeAPI.Shared.Settings;

public static class ConfigurationHelper
{
    public static void Initialize(IConfiguration configuration)
    {
        SqlConnectionString = configuration["ConnectionStrings:Local"];
    }

    public static string? SqlConnectionString { get; private set; }
}