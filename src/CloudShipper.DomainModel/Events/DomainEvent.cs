using CloudShipper.DomainModel.Aggregate;
using CloudShipper.DomainModel.Entity;

namespace CloudShipper.DomainModel.Events;

public abstract class DomainEvent<TAggregate,TAggregateId> : IDomainEvent<TAggregateId>
    where TAggregate : IEntity<TAggregateId>
{
    protected DomainEvent(TAggregateId aggregateId)
    {
        AggregateId = aggregateId;
        AggregateType = typeof(TAggregate).Name;
        AggregateTypeId = AggregateTypeIdProvider.Get(typeof(TAggregate));
        EventTypeId = DomainEventTypeIdProvider.Get(this);
    }
    public TAggregateId AggregateId { get; private set; }

    public string AggregateType { get; private set; }

    public string AggregateTypeId { get; private set; }

    public string EventTypeId { get; private set; }
}
