using CloudShipper.DomainModel.Events;

namespace CloudShipper.DomainModel.Test.Domain
{
    internal class NoDomainEventAttribute : DomainEvent<Domain.DomainObjectA,Guid>
    {
        public NoDomainEventAttribute(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
