using CloudShipper.DomainModel.Events;

namespace CloudShipper.DomainModel.Test.Domain;


[Aggregate(Constants.DomainObjectATypeId)]
internal class DomainObjectA : AggregateRoot<Guid>
{
    internal DomainObjectA(Guid id) : base(id)
    {
    }

    public void AddTestEvent()
    {
        base.AddEvent(new DomainEvent());
    }

    public void AddNullTestEvent()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        base.AddEvent(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }
}
