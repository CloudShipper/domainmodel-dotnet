namespace CloudShipper.DomainModel.Test.Domain;

[Aggregate(Constants.AuditableDomainObjectATypeId)]
internal class AuditableDomainObjectA : AuditableAggregateRoot<Guid, Guid>
{
    public AuditableDomainObjectA(Guid id, Guid principalId) : base(id, principalId)
    {
    }
}
