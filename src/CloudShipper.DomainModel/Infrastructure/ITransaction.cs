namespace CloudShipper.DomainModel.Infrastructure;

public interface ITransaction : IDisposable
{
    Task CommitAsync(CancellationToken cancellation);
    void Rollback();
}
