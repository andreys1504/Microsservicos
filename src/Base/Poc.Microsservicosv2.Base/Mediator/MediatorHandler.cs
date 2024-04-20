using MediatR;
using Newtonsoft.Json;
using Poc.Microsservicosv2.Base.Infra.RabbitMq.Publisher;
using Poc.Microsservicosv2.Base.Messages;
using Poc.Microsservicosv2.Base.Settings;
using System.Reflection;

namespace Poc.Microsservicosv2.Base.Mediator;

public sealed class MediatorHandler : IMediatorHandler
{
    private readonly IMediator _mediator;
    private readonly EnvironmentSettings _environmentSettings;
    private readonly IPublisherEventRabbitMq _publisherEventRabbitMq;
    private readonly IPublisherCommandRabbitMq _publisherCommandRabbitMq;

    public MediatorHandler(
        IMediator mediator,
        EnvironmentSettings environmentSettings,
        IPublisherEventRabbitMq publisherEventRabbitMq,
        IPublisherCommandRabbitMq publisherCommandRabbitMq)
    {
        _mediator = mediator;
        _environmentSettings = environmentSettings;
        _publisherEventRabbitMq = publisherEventRabbitMq;
        _publisherCommandRabbitMq = publisherCommandRabbitMq;
    }

    public async Task SendEventToHandlerAsync<TEvent>(TEvent @event)
    {
        await _mediator.Publish(notification: @event);
    }

    public async Task SendEventObjectToHandlerAsync(string serializedEvent, string eventName)
    {
        Assembly assembly = AppDomain.CurrentDomain.Load(_environmentSettings.DomainLayer);
        Type type = assembly.GetType(eventName);

        await SendEventToHandlerAsync(JsonConvert.DeserializeObject(serializedEvent, type));
    }

    public async Task SendCommandToHandlerAsync<TCommand>(TCommand @command)
    {
        await _mediator.Send(request: @command);
    }

    public async Task SendCommandObjectToHandlerAsync(string serializedCommand, string commandName)
    {
        Assembly assembly = AppDomain.CurrentDomain.Load(_environmentSettings.ApplicationLayer);
        Type type = assembly.GetType(commandName);
        await SendCommandToHandlerAsync(JsonConvert.DeserializeObject(serializedCommand, type));
    }

    //

    public async Task SendEventToQueueAsync<TEvent>(TEvent @event) where TEvent : Event
    {
        await _publisherEventRabbitMq.PublishEventAsync(@event);
    }

    public async Task SendCommandToQueueAsync<TCommand>(TCommand @command) where TCommand : Command
    {
        await _publisherCommandRabbitMq.PublishCommandAsync(@command);
    }
}
