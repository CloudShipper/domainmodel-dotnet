namespace CloudShipper.DomainModel.Infrastructure;

public interface ITransactionProvider
{
    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    bool HasActiveTransaction();
    IResilientTransaction NewResilientTransaction();
    void UseTransaction(ITransaction? transaction);
}
