using CloudShipper.DomainModel.Aggregate;

namespace CloudShipper.DomainModel.Repository;

public interface IAggregateRootRepository
{
}

public interface IAggregateRootRepository<TAggregate, TAggregateId> : 
    IAggregateRootRepository, 
    IAddAggregateRootRepository<TAggregate, TAggregateId>,
    ISaveAggregateRootRepository<TAggregate, TAggregateId>,
    IDeleteAggregateRootRepository<TAggregate, TAggregateId>,
    IQueryAggregateRootRepository<TAggregate, TAggregateId>,
    IQueryAllAggregateRootsRepository<TAggregate, TAggregateId>
        
    where TAggregate : class, IAggregateRoot<TAggregateId>
{
}
