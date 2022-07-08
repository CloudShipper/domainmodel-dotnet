using CloudShipper.DomainModel.Aggregate;

namespace CloudShipper.DomainModel.Repository;

public interface IAddAuditableAggregateRootRepository
{
}

public interface IAddAuditableAggregateRootRepository<TAggregate, TAggregateId, TPrincipalId> : IAddAuditableAggregateRootRepository
    where TAggregate : class, IAuditableAggregateRoot<TAggregateId, TPrincipalId>
{
    Task<TAggregate> AddAsync(TAggregate aggregate);
}
