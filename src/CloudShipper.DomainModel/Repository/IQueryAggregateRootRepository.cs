using CloudShipper.DomainModel.Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudShipper.DomainModel.Repository;

public interface IQueryAggregateRootRepository
{
}

public interface IQueryAggregateRootRepository<TAggregate, TAggregateId> : IQueryAggregateRootRepository
    where TAggregate : class, IAggregateRoot<TAggregateId>
{
    Task<TAggregate> GetAsync(TAggregateId aggregateId);
}
