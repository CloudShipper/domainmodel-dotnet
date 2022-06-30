namespace CloudShipper.DomainModel.Aggregate;

public interface IAggregateRootFactory<out TAggregateRoot, in TId>
    where TAggregateRoot : class, IAggregateRoot<TId>
{
    TAggregateRoot Create(TId id);
}
