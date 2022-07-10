using CloudShipper.DomainModel.Events;
using MediatR;

namespace CloudShipper.DomainModel.MediatR;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;

    public DomainEventDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Publish(IDomainEvent @event, CancellationToken cancellationToken = default) => 
        @event switch
        {
            null => throw new ArgumentNullException(nameof(@event)),
            INotification notification => _mediator.Publish(notification, cancellationToken),
            _ => throw new InvalidOperationException($"The type {@event.GetType()} does not implement {nameof(INotification)}")
        };
}