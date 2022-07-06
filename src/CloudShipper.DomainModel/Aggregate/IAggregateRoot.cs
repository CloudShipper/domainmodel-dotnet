using CloudShipper.DomainModel.Entity;

namespace CloudShipper.DomainModel.Aggregate;

public interface IAggregateRoot<out TAggregateId> : IAggregate, IEntity<TAggregateId>
{
    
}
