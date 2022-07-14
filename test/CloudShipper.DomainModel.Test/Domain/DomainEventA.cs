using CloudShipper.DomainModel.Events;

namespace CloudShipper.DomainModel.Test.Domain
{
    [DomainEvent(Constants.DomainEventA)]
    internal class DomainEventA : DomainEvent<Domain.DomainObjectA,Guid>
    {
        public DomainEventA(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}
