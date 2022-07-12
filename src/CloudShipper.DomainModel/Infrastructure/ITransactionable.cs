namespace CloudShipper.DomainModel.Infrastructure;

public interface ITransactionable
{
    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    bool HasActiveTransaction();
    IResilientTransaction NewResilientTransaction();
    Task SaveChangesAsync(ITransaction transaction, CancellationToken cancellationToken = default);
}
