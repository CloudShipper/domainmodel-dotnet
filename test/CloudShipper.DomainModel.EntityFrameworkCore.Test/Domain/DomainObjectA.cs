using CloudShipper.DomainModel.Aggregate;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain.Events.DomainObjectA;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain;

[Aggregate("7934B10B-9575-4966-A477-8DD1F2BCE140")]
internal class DomainObjectA : AggregateRoot<Guid>
{
    public DomainObjectA(Guid id) : base(id)
    {
        AddEvent(new CreatedEvent(Id));
    }

    public int Value1 { get; internal set; } = 1;

    public void SetValue1(int value)
    {
        Value1 = value;
        AddEvent(new Value1ChangedEvent(Id) { Value1 = value });
    }
}
