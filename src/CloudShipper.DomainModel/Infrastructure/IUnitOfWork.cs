namespace CloudShipper.DomainModel.Infrastructure;

public interface IUnitOfWork<TContext>
    where TContext : class, IContext
{
    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellation);
    TContext Context { get; }
}
