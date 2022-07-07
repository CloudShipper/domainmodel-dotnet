using CloudShipper.DomainModel.Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudShipper.DomainModel.Repository
{
    public interface IDeleteAggregateRootRepository
    {
    }

    public interface IDeleteAggregateRootRepository<TAggregate, TAggragetId> : IDeleteAggregateRootRepository 
        where TAggregate : class, IAggregateRoot<TAggragetId>
    {
        Task<TAggregate> DeleteAsync(TAggragetId aggregateId);
        Task<TAggregate> DeleteAsync(TAggregate aggregate);
    }
}
