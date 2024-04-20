using Microsoft.Extensions.Configuration;
using Poc.Microsservicosv2.Base.Settings;

namespace Poc.Microsservicosv2.Catalogo.Ioc;

public sealed class ObjectEnvironmentSettings
{
    public static EnvironmentSettings Create(IConfiguration configuration, string currentProject, string currentProjectShortName)
    {
        return Base.PresentationLayer.Build.ObjectEnvironmentSettings.Create(
           configuration,
           currentProject: currentProject,
           currentProjectShortName: currentProjectShortName,
           currentContext: nameof(Contexts.Catalogo),
           applicationLayer: "Poc.Microsservicosv2.Catalogo.Application",
           domainLayer: "Poc.Microsservicosv2.Catalogo.Domain");
    }
}
