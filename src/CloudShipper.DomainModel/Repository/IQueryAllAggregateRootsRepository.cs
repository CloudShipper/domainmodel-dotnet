using CloudShipper.DomainModel.Aggregate;

namespace CloudShipper.DomainModel.Repository;

public interface IQueryAllAggregateRootsRepository
{
}

public interface IQueryAllAggregateRootsRepository<TAggregate, TAggregateId> : IQueryAllAggregateRootsRepository
    where TAggregate : class, IAggregateRoot<TAggregateId>
{
    Task<IEnumerable<TAggregate>> GetAllAsync();
}
