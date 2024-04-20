namespace Poc.Microsservicosv2.Base.Settings.RabbitMq;

public sealed record ConnectionFactoryRabbitMqSettings
{
    public int NetworkRecoveryIntervalSeconds { get; set; }
    public bool AutomaticRecoveryEnabled { get; set; }
    public bool DispatchConsumersAsync { get; set; }
    public string HostName { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
