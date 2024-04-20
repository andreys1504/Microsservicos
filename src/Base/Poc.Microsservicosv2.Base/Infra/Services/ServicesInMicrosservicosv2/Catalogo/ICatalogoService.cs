namespace Poc.Microsservicosv2.Base.Infra.Services.ServicesInMicrosservicosv2.Catalogo;

public interface ICatalogoService
{
    Task<decimal> PrecoProduto(Guid idProduto);
}
