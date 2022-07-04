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
        base.AddEvent(null);
    }
}
