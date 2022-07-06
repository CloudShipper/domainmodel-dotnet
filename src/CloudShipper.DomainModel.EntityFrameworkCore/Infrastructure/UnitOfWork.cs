using CloudShipper.DomainModel.Aggregate;
using CloudShipper.DomainModel.Events;
using CloudShipper.DomainModel.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Infrastructure;

public class UnitOfWork<TContext> : IUnitOfWork<TContext>, ITransactionable, ITransactionHandler
    where TContext : DbContext
{
    private readonly TContext _context;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(TContext context, IDomainEventDispatcher domainEventDispatcher)
    {
        _context = context;
        _transaction = null;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public TContext Context => _context;

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (null == _transaction)
        {
            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            return new Transaction(_transaction, this);
        }

        return new Transaction(null, this);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // collect Aggregates which has events stored
        var aggregates = _context.ChangeTracker
            .Entries<IAggregate>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents != null && e.DomainEvents.Any())
            .ToList();

        var events = aggregates
            .SelectMany(a => a.DomainEvents)
            .ToList();

        // clear all events
        aggregates.ForEach(a => a.ClearEvents());

        // publish all events
        foreach (var e in events)
            await _domainEventDispatcher.Publish(e);

        // save changes
        await Context.SaveChangesAsync(cancellationToken);
    }

    async Task ITransactionHandler.CommitTransactionAsync(Transaction transaction, CancellationToken cancellation)
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
            ((ITransactionHandler)this).RollbackTransaction(transaction);
            throw;
        }
        finally 
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    void ITransactionHandler.RollbackTransaction(Transaction transaction)
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
