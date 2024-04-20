using Microsoft.AspNetCore.Http;
using Poc.Microsservicosv2.Base.Settings;
using System.Net;

namespace Poc.Microsservicosv2.Base.Infra.Authentication.RequestsBetweenApis;

public class ValidateApiKeyService
{
    public static void Validade(HttpContext context, EnvironmentSettings environmentSettings)
    {
        string apiKeyInHeader = context.Request.Headers["X-API-KEY"];
        if (string.IsNullOrWhiteSpace(apiKeyInHeader) == false)
        {
            string apiKey = environmentSettings.Authentication.RequestsBetweenApis.ApiKey;

            if (apiKeyInHeader != apiKey)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
        }
    }
}
