using CloudShipper.DomainModel.Events;

namespace CloudShipper.DomainModel.Test.Domain
{
    [DomainEvent(Constants.AuditableDomainEventA)]
    internal class AuditableDomainEventA : AuditableDomainEvent<Guid, Guid>
    {
        public AuditableDomainEventA(Guid aggregateId,
                                     string aggregateType,
                                     string aggregateTypeId,
                                     Guid principalid) : base(aggregateId, aggregateType, aggregateTypeId, principalid)
        {
        }
    }
}
