namespace CloudShipper.DomainModel.Aggregate;

public interface IAuditableAggregateRootFactory<out TAggregateRoot, in TAggregateId, in TPrincipalId>
    where TAggregateRoot : IAuditableAggregateRoot<TAggregateId, TPrincipalId>
{
    TAggregateRoot Create(TAggregateId id, TPrincipalId principalId);
}
