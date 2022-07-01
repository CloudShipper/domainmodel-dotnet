namespace CloudShipper.DomainModel.Infrastructure;

public interface ITransaction : IDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken));
    void Rollback();
}
