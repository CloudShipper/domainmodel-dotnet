using CloudShipper.DomainModel.Aggregate;

namespace CloudShipper.DomainModel.Repository;

public interface IQueryAllAuditableAggregateRootsRepository
{
}

public interface IQueryAllAuditableAggregateRootsRepository<TAggregate, TAggregateId, TPrincipalId> : IQueryAllAuditableAggregateRootsRepository
    where TAggregate : class, IAuditableAggregateRoot<TAggregateId, TPrincipalId>
{
    Task<IEnumerable<TAggregate>> GetAllAsync();
}
