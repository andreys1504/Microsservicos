namespace Poc.Microsservicosv2.Base.Settings.Authentication;

public sealed record AuthenticationSettings
{
    public RequestsBetweenApisSettings RequestsBetweenApis { get; set; } = new RequestsBetweenApisSettings();
}