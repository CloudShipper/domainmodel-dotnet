using CloudShipper.DomainModel.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Infrastructure
{
    internal class Transaction<TContext> : ITransaction
        where TContext : DbContext
    {
        private IDbContextTransaction? _dbContextTransaction;
        private readonly UnitOfWork<TContext> _unitOfWork;

        public Transaction(IDbContextTransaction? dbContextTransaction, UnitOfWork<TContext> unitOfWork)
        {
            _dbContextTransaction = dbContextTransaction;
            _unitOfWork = unitOfWork;
        }

        internal IDbContextTransaction? DbContextTransaction => _dbContextTransaction;

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _unitOfWork.CommitTransactionAsync(this, cancellationToken);
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
                _unitOfWork.RollbackTransaction(this);
            }
            finally 
            {
                _dbContextTransaction = null;
            }
            
        }
    }
}
