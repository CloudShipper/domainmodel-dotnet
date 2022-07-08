using CloudShipper.DomainModel.Aggregate;

namespace CloudShipper.DomainModel.Repository
{
    public interface IDeleteAuditableAggregateRootRepository
    {
    }

    public interface IDeleteAuditableAggregateRootRepository<TAggregate, TAggragetId, TPrincipalId> : IDeleteAuditableAggregateRootRepository
        where TAggregate : class, IAuditableAggregateRoot<TAggragetId, TPrincipalId>
    {
        Task<TAggregate> DeleteAsync(TAggragetId aggregateId);
        Task<TAggregate> DeleteAsync(TAggregate aggregate);
    }
}
