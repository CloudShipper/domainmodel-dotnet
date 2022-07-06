using CloudShipper.DomainModel.Entity;
using CloudShipper.DomainModel.Events;

namespace CloudShipper.DomainModel.Aggregate;

public interface IAuditableAggregateRoot<out TAggregateId, out TPrincipalId> : IAggregate, IEntity<TAggregateId>, IAuditable<TPrincipalId>
{
}
