﻿namespace CloudShipper.DomainModel.Events;

public abstract class DomainEvent<TAggregateId> : IDomainEvent<TAggregateId>
{
    protected DomainEvent(TAggregateId aggregateId, string aggregateType, string aggregateTypeId)
    {
        AggregateId = aggregateId;
        AggregateType = aggregateType;
        AggregateTypeId = aggregateTypeId;
        EventTypeId = DomainEventTypeIdProvider.Get(this);
    }
    public TAggregateId AggregateId { get; private set; }

    public string AggregateType { get; private set; }

    public string AggregateTypeId { get; private set; }

    public string EventTypeId { get; private set; }
}