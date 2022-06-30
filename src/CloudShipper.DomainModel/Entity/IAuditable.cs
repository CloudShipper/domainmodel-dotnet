namespace CloudShipper.DomainModel.Entity;

public interface IAuditable
{
    public DateTimeOffset? CreatedAt { get; }
    public DateTimeOffset? ModifiedAt { get; }
    public DateTimeOffset? DeletedAt { get; }
}

public interface IAuditable<out TPrincipalId> : IAuditable
{
    public TPrincipalId? CreatedBy { get; }
    public TPrincipalId? ModifiedBy { get; }
    public TPrincipalId? DeletedBy { get; }
}
