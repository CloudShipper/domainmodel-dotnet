using CloudShipper.DomainModel.Events;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain.Events.DomainObjectA;

[DomainEvent("BBBB10F8-EFC1-42B7-B59A-576A54A548D7")]
internal class CreatedEvent : DomainEvent<Domain.DomainObjectA,Guid>
{
    public CreatedEvent(Guid aggregateId) 
        : base(aggregateId)
    {
       
    }
}
