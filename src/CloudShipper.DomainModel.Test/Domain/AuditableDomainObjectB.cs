namespace CloudShipper.DomainModel.Test.Domain;

internal class AuditableDomainObjectB : AuditableAggregateRoot<Guid, Guid>
{
    public AuditableDomainObjectB(Guid id, Guid principalId) : base(id, principalId)
    {
    }
}
