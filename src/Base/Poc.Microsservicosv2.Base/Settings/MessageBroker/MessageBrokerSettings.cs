using Poc.Microsservicosv2.Base.Settings.MessageBroker.DefaultConfigs;

namespace Poc.Microsservicosv2.Base.Settings.MessageBroker;

public sealed record MessageBrokerSettings
{
    public DefaultConfigsSettings DefaultConfigs { get; set; } = new DefaultConfigsSettings();
}