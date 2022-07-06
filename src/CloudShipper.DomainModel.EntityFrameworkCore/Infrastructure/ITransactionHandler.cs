using Microsoft.EntityFrameworkCore;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Infrastructure
{
    internal interface ITransactionHandler
    {
        Task CommitTransactionAsync(Transaction transaction, CancellationToken cancellation = default);
        void RollbackTransaction(Transaction transaction);
    }
}
