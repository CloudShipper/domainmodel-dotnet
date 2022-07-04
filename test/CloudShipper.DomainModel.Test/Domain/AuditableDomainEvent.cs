using CloudShipper.DomainModel.Events;

namespace CloudShipper.DomainModel.Test.Domain;

internal class AuditableDomainEvent : IAuditableDomainEvent<Guid, Guid>
{
    public Guid RaisedBy => throw new NotImplementedException();

    public DateTimeOffset Timestamp => throw new NotImplementedException();

    public Guid AggregateId => throw new NotImplementedException();

    public string AggregateType => throw new NotImplementedException();

    public string AggregateTypeId => throw new NotImplementedException();

    public string EventTypeId => throw new NotImplementedException();
}
