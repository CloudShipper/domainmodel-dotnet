using CloudShipper.DomainModel.Aggregate;

namespace CloudShipper.DomainModel.Repository;

public interface ISaveAggregateRootRepository
{
}

public interface ISaveAggregateRootRepository<TAggregate, TAggregateId> : ISaveAggregateRootRepository
    where TAggregate : class, IAggregateRoot<TAggregateId>
{
    Task SaveAsync(TAggregate aggregate);
}
