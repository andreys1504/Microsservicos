using Microsoft.AspNetCore.Builder;

namespace Poc.Microsservicosv2.Base.PresentationLayer.AspNet.Authentication.RequestsBetweenApis;

public static class Middlewares
{
    public static void Register(WebApplication app)
    {
        app.UseMiddleware<ValidateApiKeyMiddleware>();
    }
}
