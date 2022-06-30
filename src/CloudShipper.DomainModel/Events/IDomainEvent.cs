namespace CloudShipper.DomainModel.Events;

public interface IDomainEvent
{
    string AggregateType { get; }
    string AggregateTypeId { get; }
    string EventTypeId { get; }
}

public interface IDomainEvent<out TAggregateId> : IDomainEvent
{
    TAggregateId AggregateId { get; }    
}
