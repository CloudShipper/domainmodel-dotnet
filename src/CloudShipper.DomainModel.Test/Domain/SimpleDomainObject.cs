namespace CloudShipper.DomainModel.Test.Domain;

[Aggregate(Constants.SimpleDomainObject)]
internal class SimpleDomainObject : AggregateRoot<Guid>
{
    public SimpleDomainObject(Guid id) : base(id)
    {
    }
}
