namespace CloudShipper.DomainModel.Infrastructure;

public interface IUnitOfWork<TContext>
    where TContext : class
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    TContext Context { get; }
}
