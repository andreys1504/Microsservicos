using Poc.Microsservicosv2.Base.Infra.Services.ServicesInMicrosservicosv2.Catalogo;

namespace Poc.Microsservicosv2.Infra.Services.ServicesInMicrosservicosv2.Catalogo;

public sealed class CatalogoService : ServicesInMicrosservicosv2Base, ICatalogoService
{
    private readonly HttpClient _httpClient;

    public CatalogoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal> PrecoProduto(Guid idProduto)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"/produto/preco-produto/{idProduto}");
        ValidateResponse(response);

        return await ReadResponseAsync<decimal>(response);
    }
}