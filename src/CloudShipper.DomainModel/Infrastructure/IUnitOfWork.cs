namespace CloudShipper.DomainModel.Infrastructure;

public interface IUnitOfWork<TContext>
    where TContext : class, IContext
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    TContext Context { get; }
}
