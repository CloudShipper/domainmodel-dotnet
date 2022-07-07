using CloudShipper.DomainModel.Aggregate;

namespace CloudShipper.DomainModel.Repository;

public interface IAddAggregateRootRepository
{
}

public interface IAddAggregateRootRepository<TAggregate, TAggregateId> : IAddAggregateRootRepository
    where TAggregate : class, IAggregateRoot<TAggregateId>
{
    Task<TAggregate> AddAsync(TAggregate aggregate);
}
