using CloudShipper.DomainModel.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Infrastructure
{
    internal class Transaction : ITransaction
    {
        private IDbContextTransaction? _dbContextTransaction;
        private readonly ITransactionHandler _transactionHandler;
        private bool _committed = false;
        private bool _rolledback = false;

        public Transaction(IDbContextTransaction? dbContextTransaction, ITransactionHandler transactionHandler)
        {
            _dbContextTransaction = dbContextTransaction;
            _transactionHandler = transactionHandler;
        }

        internal IDbContextTransaction? DbContextTransaction => _dbContextTransaction;

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_committed)
                throw new InvalidOperationException("Transaction already stale.");

            try
            {
                await _transactionHandler.CommitTransactionAsync(this, cancellationToken);
            }            
            finally 
            {
                _committed = true;
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
            if (_rolledback)
                throw new InvalidOperationException("Transaction already stale.");

            try
            {
                _transactionHandler.RollbackTransaction(this);
            }
            finally 
            {
                _rolledback = true;
                _dbContextTransaction = null;
            }            
        }
    }
}
