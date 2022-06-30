namespace CloudShipper.DomainModel.Entity;

public interface ITenantEntity<out TTenantId, out TId> : IEntity<TId>
{
    TTenantId TenantId { get; }
}
