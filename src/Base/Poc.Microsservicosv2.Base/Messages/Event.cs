namespace Poc.Microsservicosv2.Base.Messages;

public abstract class Event : Message
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
