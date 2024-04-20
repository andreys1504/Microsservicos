namespace Poc.Microsservicosv2.Base.Infra.Services.Logger.Models;

public sealed record LogErrorModel
{
    public required Exception Exception { get; init; }
    public required string Message { get; init; }
    public required string Keyword { get; init; }
    public LevelLogError Level { get; init; } = LevelLogError.ERROR;
}
