using CloudShipper.DomainModel.Events;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain.Events.DomainObjectA;

[DomainEvent("5DF5F67D-4DF8-4712-89AA-247C204514D7")]
internal class Value1ChangedEvent : DomainEvent<Guid>
{
    public Value1ChangedEvent(
        Guid aggregateId, Type aggregateType) 
        : base(aggregateId, aggregateType)
    {
    }

    public int Value1 { get; set; }
}
