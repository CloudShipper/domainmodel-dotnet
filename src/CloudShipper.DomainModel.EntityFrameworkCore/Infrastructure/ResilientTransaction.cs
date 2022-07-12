using CloudShipper.DomainModel.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Infrastructure;

internal class ResilientTransaction : IResilientTransaction
{
    private readonly ITransactionable _context;
    private readonly DbContext _dbContext;

    public ResilientTransaction(ITransactionable context, DbContext dbContext)
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

            try
            {
                await action();
                await tx.CommitAsync();
            }
            catch (Exception)
            {
                tx.Rollback();
                throw;
            }
        });
    }
}
