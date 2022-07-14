using CloudShipper.DomainModel.Aggregate;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain.Events.DomainObjectA;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain
{
    [Aggregate("CD469D0F-E45E-4115-A92E-F9D8439A807E")]
    internal class DomainObjectB : AggregateRoot<Guid>
    {
        public DomainObjectB(Guid id) : base(id)
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
}
