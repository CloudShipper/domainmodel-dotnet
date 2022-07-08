using CloudShipper.DomainModel.Aggregate;

namespace CloudShipper.DomainModel.Repository;

public interface IAuditableAggregateRootRepository
{
}

public interface IAuditableAggregateRootRepository<TAggregate, TAggregateId, TPrincipalId> :
    IAuditableAggregateRootRepository,
    IAddAuditableAggregateRootRepository<TAggregate, TAggregateId, TPrincipalId>,
    ISaveAuditableAggregateRootRepository<TAggregate, TAggregateId, TPrincipalId>,
    IDeleteAuditableAggregateRootRepository<TAggregate, TAggregateId, TPrincipalId>,
    IQueryAuditableAggregateRootRepository<TAggregate, TAggregateId, TPrincipalId>,
    IQueryAllAuditableAggregateRootsRepository<TAggregate, TAggregateId, TPrincipalId>
        
    where TAggregate : class, IAuditableAggregateRoot<TAggregateId, TPrincipalId>
{
}
