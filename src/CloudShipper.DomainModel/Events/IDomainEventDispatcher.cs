namespace CloudShipper.DomainModel.Events;

public interface IDomainEventDispatcher
{
    Task Publish(IDomainEvent @event, CancellationToken cancellationToken = default);
}
