using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Poc.Microsservicosv2.Base.Settings;

namespace Poc.Microsservicosv2.Base.PresentationLayer.Build;

public static class AppSettingsJsonBuildConfigurations
{
    public static string GetCurrentEnvironmentName(IConfiguration configuration)
    {
        return configuration.GetSection("EnvironmentSettings:CurrentEnvironment").Value;
    }

    public static void GetConfigurations(IConfiguration configuration, EnvironmentSettings environmentSettings)
    {
        IConfigurationSection environmentSettingsSection = configuration.GetSection(nameof(EnvironmentSettings));
        new ConfigureFromConfigurationOptions<EnvironmentSettings>(environmentSettingsSection).Configure(environmentSettings);
    }

    public static string GetCurrentEnvironmentName(IConfiguration configuration, string currentEnvironmentKey)
    {
        string currentEnvironment = configuration[currentEnvironmentKey];
        if (string.IsNullOrWhiteSpace(currentEnvironment))
            currentEnvironment = configuration.GetSection("EnvironmentSettings:CurrentEnvironment").Value;

        return currentEnvironment;
    }
}
