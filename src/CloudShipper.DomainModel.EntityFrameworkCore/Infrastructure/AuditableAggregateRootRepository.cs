using CloudShipper.DomainModel.Aggregate;
using CloudShipper.DomainModel.Infrastructure;
using CloudShipper.DomainModel.Repository;
using Microsoft.EntityFrameworkCore;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Infrastructure
{
    public class AuditableAggregateRootRepository<TDbContext, TAggregate, TAggregateId, TPrincipalId> : 
            IAuditableAggregateRootRepository<TAggregate, TAggregateId, TPrincipalId>
        where TAggregate : class, IAuditableAggregateRoot<TAggregateId, TPrincipalId>
        where TDbContext : DbContext
        where TAggregateId : IEquatable<TAggregateId>
    {
        private readonly IUnitOfWork<TDbContext> _unitOfWork;
        private readonly DbSet<TAggregate> _aggregateSet;

        public AuditableAggregateRootRepository(IUnitOfWork<TDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _aggregateSet = _unitOfWork.Context.Set<TAggregate>();
        }

        internal IUnitOfWork<TDbContext> UnitOfWork => _unitOfWork;

        public async Task<TAggregate> AddAsync(TAggregate aggregate)
        {
            if (null == aggregate)
                throw new ArgumentNullException(nameof(aggregate));

            await _aggregateSet.AddAsync(aggregate);

            await _unitOfWork.SaveChangesAsync();

            return aggregate;
        }

        public async Task<TAggregate> DeleteAsync(TAggregateId aggregateId)
        {
            TAggregate? aggregate = await _aggregateSet
                .SingleOrDefaultAsync(a => a.Id.Equals(aggregateId));

            if (null == aggregate)
                throw new KeyNotFoundException();

            _aggregateSet.Remove(aggregate);

            await _unitOfWork.SaveChangesAsync();

            return aggregate;
        }

        public async Task<TAggregate> DeleteAsync(TAggregate aggregate)
        {
            if (null == aggregate)
                throw new ArgumentNullException(nameof(aggregate));

            _aggregateSet.Remove(aggregate);
            await _unitOfWork.SaveChangesAsync();
            return aggregate;
        }

        public async Task<IEnumerable<TAggregate>> GetAllAsync()
        {
            var result = await _aggregateSet.ToListAsync();
            result.ForEach(x => x.ClearEvents());
            return result;
        }

        public async Task<TAggregate> GetAsync(TAggregateId aggregateId)
        {
            TAggregate? aggregate = await _aggregateSet
                .SingleOrDefaultAsync(a => a.Id.Equals(aggregateId));

            if (null == aggregate)
                throw new KeyNotFoundException();

            aggregate.ClearEvents();

            return aggregate;
        }

        public async Task SaveAsync(TAggregate aggregate)
        {
            if (null == aggregate)
                throw new ArgumentNullException(nameof(aggregate));

            _aggregateSet.Update(aggregate);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
