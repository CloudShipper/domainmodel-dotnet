namespace CloudShipper.DomainModel.Test.Domain;

[Aggregate(Constants.SimpleAuditableDomainObject)]
internal class SimpleAuditableDomainObject : AuditableAggregateRoot<Guid, Guid>
{
    public SimpleAuditableDomainObject(Guid id, Guid principalId) : base(id, principalId)
    {
    }
}
