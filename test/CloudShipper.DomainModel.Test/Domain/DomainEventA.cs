using CloudShipper.DomainModel.Events;

namespace CloudShipper.DomainModel.Test.Domain
{
    [DomainEvent(Constants.DomainEventA)]
    internal class DomainEventA : DomainEvent<Guid>
    {
        public DomainEventA(Guid aggregateId,
                            string aggregateType,
                            string aggregateTypeId) : base(aggregateId, aggregateType, aggregateTypeId)
        {
        }
    }
}
