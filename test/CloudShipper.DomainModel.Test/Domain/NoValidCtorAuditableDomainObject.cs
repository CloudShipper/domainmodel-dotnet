using CloudShipper.DomainModel.Events;

namespace CloudShipper.DomainModel.Test.Domain;

internal class NoValidCtorAuditableDomainObject : IAuditableAggregateRoot<Guid, Guid>
{
    public IReadOnlyCollection<IAuditableDomainEvent<Guid, Guid>> AuditableDomainEvents => throw new NotImplementedException();

    public string TypeId => throw new NotImplementedException();

    public Guid Id => throw new NotImplementedException();

    public Guid CreatedBy => throw new NotImplementedException();

    public Guid ModifiedBy => throw new NotImplementedException();

    public Guid DeletedBy => throw new NotImplementedException();

    public DateTimeOffset? CreatedAt => throw new NotImplementedException();

    public DateTimeOffset? ModifiedAt => throw new NotImplementedException();

    public DateTimeOffset? DeletedAt => throw new NotImplementedException();

    public void ClearEvents()
    {
        throw new NotImplementedException();
    }
}
