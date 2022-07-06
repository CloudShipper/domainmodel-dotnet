using CloudShipper.DomainModel.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Infrastructure
{
    internal class Transaction : ITransaction
    {
        private IDbContextTransaction? _dbContextTransaction;
        private readonly ITransactionHandler _transactionHandler;

        public Transaction(IDbContextTransaction? dbContextTransaction, ITransactionHandler transactionHandler)
        {
            _dbContextTransaction = dbContextTransaction;
            _transactionHandler = transactionHandler;
        }

        internal IDbContextTransaction? DbContextTransaction => _dbContextTransaction;

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _transactionHandler.CommitTransactionAsync(this, cancellationToken);
            }            
            finally 
            {
                _dbContextTransaction = null;
            }
        }

        public void Dispose()
        {
            if (null == _dbContextTransaction)
                return;

            Rollback();
        }

        public void Rollback()
        {
            try
            {
                _transactionHandler.RollbackTransaction(this);
            }
            finally 
            {
                _dbContextTransaction = null;
            }            
        }
    }
}
