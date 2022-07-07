using CloudShipper.DomainModel.Aggregate;
using CloudShipper.DomainModel.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CloudShipper.DomainModel.EntityFrameworkCore;

internal readonly record struct AggregateRootToRepositroryBinding(Type DbContextType,Type AggregateType, Type AggregateIdType);
internal readonly record struct AuditableAggregateRootToRepositroryBinding(Type DbContextType, Type AggregateType, Type AggregateIdType, Type PrincipalIdType);

internal class DbContextAggregateBinder<TDbContext> : IDbContextAggregateBinder<TDbContext>
    where TDbContext : DbContext
{
    private List<AggregateRootToRepositroryBinding> _aggregateRootBindings = new();
    private List<AuditableAggregateRootToRepositroryBinding> _auditableAggregateRootBindings = new();

    internal IReadOnlyCollection<AggregateRootToRepositroryBinding> AggregateRootBindings => _aggregateRootBindings;
    internal IReadOnlyCollection<AuditableAggregateRootToRepositroryBinding> AuditableAggregateRootBindings => _auditableAggregateRootBindings;

    public IDbContextAggregateBinder<TDbContext> Bind<TAggregate, TAggregateId>()
        where TAggregate : IAggregateRoot<TAggregateId>
    {        
        _aggregateRootBindings.Add(
            new AggregateRootToRepositroryBinding(
                typeof(TDbContext), typeof(TAggregate), typeof(TAggregateId)));

        return this;
    }

    public IDbContextAggregateBinder<TDbContext> Bind<TAggregate, TAggregateId, TPrincipalId>()
        where TAggregate : IAuditableAggregateRoot<TAggregateId, TPrincipalId>
    {
        _auditableAggregateRootBindings.Add(
            new AuditableAggregateRootToRepositroryBinding(
                typeof(TDbContext), typeof(TAggregate), typeof(TAggregateId), typeof(TPrincipalId)));

        return this;
    }
}
