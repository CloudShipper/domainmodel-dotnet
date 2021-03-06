using CloudShipper.DomainModel.Events;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain.Events.AuditableDomainObjectA;

[DomainEvent("86FA40F3-9A7C-450E-BECD-C348EA7AF480")]
internal class CreatedEvent : AuditableDomainEvent<Domain.AuditableDomainObjectA, Guid, Guid>
{
    public CreatedEvent(Guid aggregateId, Guid principalId) 
        : base(aggregateId, principalId)
    {
       
    }
}
