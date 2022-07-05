namespace CloudShipper.DomainModel.Test.Domain;

internal class NoAggergateAttributeDomainObject : AggregateRoot<Guid>
{
    public NoAggergateAttributeDomainObject(Guid id) : base(id)
    {
    }
}
