namespace CloudShipper.DomainModel.Test.Domain;

internal class DomainObjectBFactory : AggregateRootFactory<DomainObjectB, Guid>, IDomainObjectBFactory
{
    public DomainObjectB Create()
    {
        return new DomainObjectB(Guid.NewGuid());
    }
}
