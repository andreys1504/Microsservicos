namespace Poc.Microsservicosv2.Base.Settings.MessageBroker.DefaultConfigs;

public sealed record DefaultConfigsSettings
{
    public ExchangeQueueSettings Events { get; set; } = new ExchangeQueueSettings();
    public ExchangeQueueSettings Commands { get; set; } = new ExchangeQueueSettings();
}
