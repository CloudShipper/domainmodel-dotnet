namespace CloudShipper.DomainModel.Test.Domain;


[Aggregate(Constants.DomainObjectATypeId)]
internal class DomainObjectA : AggregateRoot<Guid>
{
    internal DomainObjectA(Guid id) : base(id)
    {
    }
}
