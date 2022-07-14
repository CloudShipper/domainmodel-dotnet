using CloudShipper.DomainModel.Events;

namespace CloudShipper.DomainModel.Test.Domain
{
    [DomainEvent(Constants.AuditableDomainEventA)]
    internal class AuditableDomainEventA : AuditableDomainEvent<Domain.AuditableDomainObjectA, Guid, Guid>
    {
        public AuditableDomainEventA(Guid aggregateId,                                     
                                     Guid principalid) : base(aggregateId, principalid)
        {
        }
    }
}
