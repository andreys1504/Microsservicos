using Microsoft.Extensions.DependencyInjection;
using Poc.Microsservicosv2.Base.Infra.Services.ServicesInMicrosservicosv2.Catalogo;
using Poc.Microsservicosv2.Base.ResiliencePolicies;
using Poc.Microsservicosv2.Base.Settings;
using Poc.Microsservicosv2.Infra.Services.ServicesInMicrosservicosv2.Catalogo;

namespace Poc.Microsservicosv2.Infra.Services.ServicesInMicrosservicosv2;

public static class Dependencies
{
    public static void Register(IServiceCollection services, EnvironmentSettings environmentSettings)
    {
        services
            .AddHttpClient<ICatalogoService, CatalogoService>(httpClient => SetBasicSettings(httpClient, environmentSettings, Contexts.Catalogo))
            .AddHttpResponseWaitAndRetryPolicyHandler();
    }

    //
    private static void SetBasicSettings(HttpClient httpClient, EnvironmentSettings environmentSettings, Contexts context)
    {
        httpClient.BaseAddress = new Uri(environmentSettings.ServicesInMicrosservicosv2.First(srv => srv.Service == context.ToString()).BaseUrl);
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        httpClient.DefaultRequestHeaders.Add("X-API-KEY", environmentSettings.Authentication.RequestsBetweenApis.ApiKey);
    }
}
