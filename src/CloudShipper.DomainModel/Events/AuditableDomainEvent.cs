namespace CloudShipper.DomainModel.Events
{
    public abstract class AuditableDomainEvent<TAggregateId, TPrincipalId> : 
        DomainEvent<TAggregateId>, IAuditableDomainEvent<TAggregateId, TPrincipalId>
    {
        protected AuditableDomainEvent(TAggregateId aggregateId, string aggregateType, string aggregateTypeId, TPrincipalId principalid) 
            : base(aggregateId, aggregateType, aggregateTypeId)
        {
            RaisedBy = principalid;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public TPrincipalId RaisedBy { get; private set; }

        public DateTimeOffset Timestamp { get; private set; }
    }
}
