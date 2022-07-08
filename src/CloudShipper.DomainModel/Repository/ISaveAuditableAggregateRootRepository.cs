using CloudShipper.DomainModel.Aggregate;

namespace CloudShipper.DomainModel.Repository;

public interface ISaveAuditableAggregateRootRepository
{
}

public interface ISaveAuditableAggregateRootRepository<TAggregate, TAggregateId, TPrincipalId> : ISaveAuditableAggregateRootRepository
    where TAggregate : class, IAuditableAggregateRoot<TAggregateId, TPrincipalId>
{
    Task SaveAsync(TAggregate aggregate);
}
