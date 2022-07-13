using CloudShipper.DomainModel.Aggregate;
using CloudShipper.DomainModel.Events;
using CloudShipper.DomainModel.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Infrastructure;

public class UnitOfWork<TContext> : IUnitOfWork<TContext>, ITransactionProvider
    where TContext : DbContext
{
    private readonly TContext _context;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public UnitOfWork(TContext context, IDomainEventDispatcher domainEventDispatcher)
    {
        _context = context;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public TContext Context => _context;

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var tx = await _context.Database.BeginTransactionAsync(cancellationToken);
        return new Transaction(tx);
    }

    public IResilientTransaction NewResilientTransaction()
    {
        return new ResilientTransaction(this, _context);
    }

    public bool HasActiveTransaction()
    {
        return null != _context.Database.CurrentTransaction;
    }

    public void UseTransaction(ITransaction? transaction)
    {
        if (null == transaction)
        {
            _context.Database.UseTransaction(null);
            return;
        }

        var tx = transaction as Transaction;
        if (null == tx)
            throw new InvalidOperationException();

        _context.Database.UseTransaction(tx.DbContextTransaction.GetDbTransaction(), tx.TransactionId);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // collect Aggregates which has events stored
        var aggregates = _context.ChangeTracker
            .Entries<IAggregate>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Count > 0)
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
}
