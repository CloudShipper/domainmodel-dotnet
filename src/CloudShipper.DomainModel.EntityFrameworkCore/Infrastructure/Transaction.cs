using CloudShipper.DomainModel.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Infrastructure;

internal class Transaction : ITransaction
{
    private IDbContextTransaction _dbContextTransaction;        
    private bool _committed = false;
    private bool _rolledback = false;

    public Transaction(IDbContextTransaction dbContextTransaction)
    {
        _dbContextTransaction = dbContextTransaction;
    }

    internal IDbContextTransaction DbContextTransaction => _dbContextTransaction;

    public Guid TransactionId => _dbContextTransaction.TransactionId;

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_committed)
            throw new InvalidOperationException("Transaction already stale.");

        try
        {
            await _dbContextTransaction.CommitAsync(cancellationToken);
        }            
        catch (Exception)
        {
            _dbContextTransaction.Rollback();
            throw;
        }
        finally 
        {
            _committed = true;
            _dbContextTransaction.Dispose();
        }
    }

    public void Dispose()
    {
        if (_committed || _rolledback)
            return;

        Rollback();
    }

    public void Rollback()
    {
        if (_rolledback)
            throw new InvalidOperationException("Transaction already stale.");

        try
        {
            _dbContextTransaction.Rollback();
        }            
        finally 
        {
            _rolledback = true;
            _dbContextTransaction.Dispose();
        }            
    }
}
