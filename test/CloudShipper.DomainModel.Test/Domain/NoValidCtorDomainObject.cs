using CloudShipper.DomainModel.Events;

namespace CloudShipper.DomainModel.Test.Domain;

internal class NoValidCtorDomainObject : IAggregateRoot<Guid>
{
    public IReadOnlyCollection<IDomainEvent> DomainEvents => throw new NotImplementedException();

    public string TypeId => throw new NotImplementedException();

    public Guid Id => throw new NotImplementedException();

    public void ClearEvents()
    {
        throw new NotImplementedException();
    }
}
