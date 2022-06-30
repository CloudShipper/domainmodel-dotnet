namespace CloudShipper.DomainModel.Events;

public interface IAuditableDomainEvent : IDomainEvent
{
    DateTimeOffset Timestamp { get; }
}

public interface IAuditableDomainEvent<out TAggregateId, out TPrincipalId> : IAuditableDomainEvent, IDomainEvent<TAggregateId>
{
    TPrincipalId RaisedBy { get; }
}
