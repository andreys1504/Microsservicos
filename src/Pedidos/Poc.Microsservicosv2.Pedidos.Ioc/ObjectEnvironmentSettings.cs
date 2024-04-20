using Microsoft.Extensions.Configuration;
using Poc.Microsservicosv2.Base.Settings;

namespace Poc.Microsservicosv2.Pedidos.Ioc;

public sealed class ObjectEnvironmentSettings
{
    public static EnvironmentSettings Create(IConfiguration configuration, string currentProject, string currentProjectShortName)
    {
        return Base.PresentationLayer.Build.ObjectEnvironmentSettings.Create(
           configuration,
           currentProject: currentProject,
           currentProjectShortName: currentProjectShortName,
           currentContext: nameof(Contexts.Pedidos),
           applicationLayer: "Poc.Microsservicosv2.Pedidos.Application",
           domainLayer: "Poc.Microsservicosv2.Pedidos.Domain");
    }
}
