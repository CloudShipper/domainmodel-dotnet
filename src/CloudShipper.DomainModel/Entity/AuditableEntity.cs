namespace CloudShipper.DomainModel.Entity;

public abstract class AuditableEntity<TId, TPrincipalId> : Entity<TId>, IAuditable<TPrincipalId>
{
    protected AuditableEntity(TId id, TPrincipalId principalId) : base(id)
    {
        CreatedBy = principalId;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public DateTimeOffset? CreatedAt { get; protected set; }
    public DateTimeOffset? ModifiedAt { get; protected set; }
    public DateTimeOffset? DeletedAt { get; protected set; }

    public TPrincipalId? CreatedBy { get; protected set; }
    public TPrincipalId? ModifiedBy { get; protected set; }
    public TPrincipalId? DeletedBy { get; protected set; }
}
