using Microsoft.Extensions.Configuration;
using Poc.Microsservicosv2.Base.Settings;

namespace Poc.Microsservicosv2.Base.PresentationLayer.Build;

public static class ObjectEnvironmentSettings
{
    public static EnvironmentSettings Create(
        IConfiguration configuration,
        string currentProject,
        string currentProjectShortName,
        string currentContext,
        string applicationLayer,
        string domainLayer)
    {
        string currentEnvironment = configuration["CURRENT_ENVIRONMENT"];

        bool currentEnvironmentExists = true;
        if (string.IsNullOrWhiteSpace(currentEnvironment))
        {
            currentEnvironmentExists = false;
            currentEnvironment = AppSettingsJsonBuildConfigurations.GetCurrentEnvironmentName(configuration);
        }

        var environmentSettings = new EnvironmentSettings(
            currentEnvironment: currentEnvironment,
            currentProject: currentProject,
            currentProjectShortName: currentProjectShortName,
            currentContext: currentContext,
            applicationLayer: applicationLayer,
            domainLayer: domainLayer);

        //não recuperar configs de appsettings.json, pois CURRENT_ENVIRONMENT foi setado
        if (currentEnvironmentExists == false)
            AppSettingsJsonBuildConfigurations.GetConfigurations(configuration, environmentSettings);

        environmentSettings.SetOtherSettings(configuration);

        return environmentSettings;
    }
}
