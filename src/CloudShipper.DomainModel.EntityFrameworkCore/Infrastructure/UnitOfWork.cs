using CloudShipper.DomainModel.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Infrastructure;

public class UnitOfWork<TContext> : IUnitOfWork<TContext>, ITransactionable
    where TContext : DbContext
{
    private readonly TContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(TContext context)
    {
        _context = context;
        _transaction = null;
    }

    public TContext Context => _context;

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (null == _transaction)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            return new Transaction<TContext>(_transaction, this);
        }

        return new Transaction<TContext>(null, this);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // todo: dispatch Domain Events

        // save changes
        await Context.SaveChangesAsync(cancellationToken);
    }

    internal async Task CommitTransactionAsync(Transaction<TContext> transaction, CancellationToken cancellation = default)
    {
        if (null == transaction)
            throw new ArgumentNullException(nameof(transaction));

        // is it the first transaction?
        if (null == transaction.DbContextTransaction)
            return;

        // right context?
        if (transaction.DbContextTransaction != _transaction)
            throw new InvalidOperationException($"Transaction is not the current");

        try
        {
            await Context.SaveChangesAsync();
            await transaction.DbContextTransaction.CommitAsync(cancellation);
        }
        catch
        {
            RollbackTransaction(transaction);
            throw;
        }
        finally 
        {
            _transaction?.Dispose();
            _transaction = null;
        }
        
    }

    internal void RollbackTransaction(Transaction<TContext> transaction)
    {
        if (null == transaction)
            throw new ArgumentNullException();

        if (null == transaction.DbContextTransaction)
            return;

        if (transaction.DbContextTransaction != _transaction)
            throw new InvalidOperationException("Transaction is not the current");

        try
        {
            transaction.DbContextTransaction.Rollback();
        }
        catch
        {
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }
}
