using System.Net.Http.Json;

namespace Poc.Microsservicosv2.Infra.Services.ServicesInMicrosservicosv2;

public abstract class ServicesInMicrosservicosv2Base
{
    protected void ValidateResponse(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
    }

    protected async Task<TResponse> ReadResponseAsync<TResponse>(HttpResponseMessage response)
    {
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }
}
