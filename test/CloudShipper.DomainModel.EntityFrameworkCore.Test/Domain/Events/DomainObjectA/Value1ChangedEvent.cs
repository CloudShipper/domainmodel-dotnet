using CloudShipper.DomainModel.Events;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain.Events.DomainObjectA;

[DomainEvent("5DF5F67D-4DF8-4712-89AA-247C204514D7")]
internal class Value1ChangedEvent : DomainEvent<Domain.DomainObjectA, Guid>
{
    public Value1ChangedEvent(
        Guid aggregateId) 
        : base(aggregateId)
    {
    }

    public int Value1 { get; set; }
}
