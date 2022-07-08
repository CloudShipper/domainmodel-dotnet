using CloudShipper.DomainModel.Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudShipper.DomainModel.Repository;

public interface IQueryAuditableAggregateRootRepository
{
}

public interface IQueryAuditableAggregateRootRepository<TAggregate, TAggregateId, TPrincipalId> : IQueryAuditableAggregateRootRepository
    where TAggregate : class, IAuditableAggregateRoot<TAggregateId, TPrincipalId>
{
    Task<TAggregate> GetAsync(TAggregateId aggregateId);
}
