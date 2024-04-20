namespace Poc.Microsservicosv2.Base.Infra.RabbitMq;

public static partial class Extensions
{
    public static TimeSpan AsMessageRateToSleepTimeSpan(this int messagesPerSecond)
    {
        if (messagesPerSecond < 1)
            throw new ArgumentOutOfRangeException(nameof(messagesPerSecond));


        var sleepTimer = 1000 / messagesPerSecond;

        return TimeSpan.FromMilliseconds(sleepTimer);
    }

    public static TimeSpan AsMessageRateToSleepTimeSpan(this ushort messagesPerSecond)
    {
        return AsMessageRateToSleepTimeSpan((int)messagesPerSecond);
    }

    public static void Wait(this TimeSpan time)
    {
        Thread.Sleep(time);
    }
}
