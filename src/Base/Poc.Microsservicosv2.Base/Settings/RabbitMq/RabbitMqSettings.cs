namespace Poc.Microsservicosv2.Base.Settings.RabbitMq;

public sealed record RabbitMqSettings
{
    public ConnectionFactoryRabbitMqSettings ConnectionFactory { get; set; } = new ConnectionFactoryRabbitMqSettings();
    public int RetryCountConnection { get; set; }
    public int RetryPublishSecondsDelay { get; set; }
}
