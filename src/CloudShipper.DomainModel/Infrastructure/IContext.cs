namespace CloudShipper.DomainModel.Infrastructure;

public interface IContext
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
}
