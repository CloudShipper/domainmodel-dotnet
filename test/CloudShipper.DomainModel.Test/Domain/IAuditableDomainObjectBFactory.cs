namespace CloudShipper.DomainModel.Test.Domain;

internal interface IAuditableDomainObjectBFactory : IAuditableAggregateRootFactory<AuditableDomainObjectB, Guid, Guid>
{
    AuditableDomainObjectB Create(Guid principalId);
}
