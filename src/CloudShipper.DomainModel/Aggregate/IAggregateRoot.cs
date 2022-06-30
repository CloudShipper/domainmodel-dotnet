using CloudShipper.DomainModel.Entity;
using CloudShipper.DomainModel.Events;

namespace CloudShipper.DomainModel.Aggregate;

public interface IAggregateRoot<out TAggregateId> : IAggregate, IEntity<TAggregateId>
{
    IReadOnlyCollection<IDomainEvent<TAggregateId>> DomainEvents { get; }
    void ClearEvents();
}
