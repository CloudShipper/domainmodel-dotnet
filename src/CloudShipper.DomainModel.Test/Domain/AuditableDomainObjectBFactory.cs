namespace CloudShipper.DomainModel.Test.Domain;

internal class AuditableDomainObjectBFactory : AuditableAggregateRootFactory<AuditableDomainObjectB, Guid, Guid>, IAuditableDomainObjectBFactory
{
    public AuditableDomainObjectB Create(Guid principalId)
    {
        return new AuditableDomainObjectB(Guid.NewGuid(), principalId);
    }
}
