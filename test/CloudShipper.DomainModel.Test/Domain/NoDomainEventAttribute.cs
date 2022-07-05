using CloudShipper.DomainModel.Events;

namespace CloudShipper.DomainModel.Test.Domain
{
    internal class NoDomainEventAttribute : DomainEvent<Guid>
    {
        public NoDomainEventAttribute(Guid aggregateId,
                                      string aggregateType,
                                      string aggregateTypeId) : base(aggregateId, aggregateType, aggregateTypeId)
        {
        }
    }
}
