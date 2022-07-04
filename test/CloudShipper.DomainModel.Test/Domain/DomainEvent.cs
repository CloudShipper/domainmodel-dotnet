using CloudShipper.DomainModel.Events;

namespace CloudShipper.DomainModel.Test.Domain;

internal class DomainEvent : IDomainEvent<Guid>
{
    public Guid AggregateId => throw new NotImplementedException();

    public string AggregateType => throw new NotImplementedException();

    public string AggregateTypeId => throw new NotImplementedException();

    public string EventTypeId => throw new NotImplementedException();
}
