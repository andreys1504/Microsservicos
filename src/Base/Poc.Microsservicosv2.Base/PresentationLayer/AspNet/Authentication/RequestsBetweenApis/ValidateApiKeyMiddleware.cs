using Microsoft.AspNetCore.Http;
using Poc.Microsservicosv2.Base.Infra.Authentication.RequestsBetweenApis;
using Poc.Microsservicosv2.Base.Settings;

namespace Poc.Microsservicosv2.Base.PresentationLayer.AspNet.Authentication.RequestsBetweenApis;

public sealed class ValidateApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly EnvironmentSettings _environmentSettings;

    public ValidateApiKeyMiddleware(
        RequestDelegate next,
        EnvironmentSettings environmentSettings)
    {
        _next = next;
        _environmentSettings = environmentSettings;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ValidateApiKeyService.Validade(context, _environmentSettings);

        await _next(context);
    }
}
