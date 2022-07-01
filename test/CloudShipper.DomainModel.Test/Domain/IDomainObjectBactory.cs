namespace CloudShipper.DomainModel.Test.Domain;

internal interface IDomainObjectBFactory : IAggregateRootFactory<DomainObjectB, Guid>
{
    DomainObjectB Create();
}
