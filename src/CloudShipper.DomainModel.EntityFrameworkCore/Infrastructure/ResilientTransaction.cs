using CloudShipper.DomainModel.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Infrastructure;

internal class ResilientTransaction : IResilientTransaction
{
    private readonly ITransactionProvider _context;
    private readonly DbContext _dbContext;

    public ResilientTransaction(ITransactionProvider context, DbContext dbContext)
    {
        _context = context;
        _dbContext = dbContext;
    }

    public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
        //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency

        var strategy = _dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            using var tx = await _context.BeginTransactionAsync(cancellationToken);

            await action();
            await tx.CommitAsync();            
        });
    }

    public async Task ExecuteAsync(Func<ITransaction, Task> action, CancellationToken cancellationToken = default)
    {
        //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
        //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency

        var strategy = _dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            using var tx = await _context.BeginTransactionAsync(cancellationToken);

            await action(tx);
            await tx.CommitAsync();            
        });
    }

    public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default, params ITransactionProvider?[] transactionProvider)
    {
        var strategy = _dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            using var tx = await _context.BeginTransactionAsync(cancellationToken);

            foreach (var transactionProvider in transactionProvider)
            {
                transactionProvider?.UseTransaction(tx);
            }

            try
            {
                await action();
                await tx.CommitAsync(cancellationToken);
            }
            finally
            {
                foreach (var transactionProvider in transactionProvider)
                {
                    transactionProvider?.UseTransaction(null);
                }
            }
        });
    }
}
