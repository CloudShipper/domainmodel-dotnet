namespace CloudShipper.DomainModel.Infrastructure;

public interface IResilientTransaction
{
    Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default);
    Task ExecuteAsync(Func<ITransaction, Task> action, CancellationToken cancellationToken = default);
}
