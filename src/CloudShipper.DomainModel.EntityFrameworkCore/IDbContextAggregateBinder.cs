using CloudShipper.DomainModel.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace CloudShipper.DomainModel.EntityFrameworkCore;

public interface IDbContextAggregateBinder<TDbContext>
    where TDbContext : DbContext
{
    IDbContextAggregateBinder<TDbContext> Bind<TAggregate, TAggregateId>()
    where TAggregate : IAggregateRoot<TAggregateId>;

    IDbContextAggregateBinder<TDbContext> Bind<TAggregate, TAggregateId, TPrincipalId>()
    where TAggregate : IAuditableAggregateRoot<TAggregateId, TPrincipalId>;
}
