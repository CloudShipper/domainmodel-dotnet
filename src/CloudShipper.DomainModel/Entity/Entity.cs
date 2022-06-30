namespace CloudShipper.DomainModel.Entity;

public abstract class Entity<TId> : IEntity<TId>
{
    protected Entity(TId id)
    {
        Id = id;
    }
    public TId Id { get; private set; }
}
