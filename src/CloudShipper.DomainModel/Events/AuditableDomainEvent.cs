using CloudShipper.DomainModel.Aggregate;

namespace CloudShipper.DomainModel.Events
{
    public abstract class AuditableDomainEvent<TAggregate, TAggregateId, TPrincipalId> : 
        DomainEvent<TAggregate, TAggregateId>, IAuditableDomainEvent<TAggregateId, TPrincipalId>
        where TAggregate : IAuditableAggregateRoot<TAggregateId, TPrincipalId>
    {

        protected AuditableDomainEvent(TAggregateId aggregateId, TPrincipalId principalid)
            : base(aggregateId)
        {
            RaisedBy = principalid;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public TPrincipalId RaisedBy { get; private set; }

        public DateTimeOffset Timestamp { get; private set; }
    }
}
