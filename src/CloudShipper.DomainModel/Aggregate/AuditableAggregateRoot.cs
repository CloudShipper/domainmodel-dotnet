using CloudShipper.DomainModel.Entity;
using CloudShipper.DomainModel.Events;
using System.Collections.Immutable;

namespace CloudShipper.DomainModel.Aggregate;

public abstract class AuditableAggregateRoot<TId, TPrincipalId> : AuditableEntity<TId, TPrincipalId>, IAuditableAggregateRoot<TId, TPrincipalId>
{
    private readonly Queue<IAuditableDomainEvent<TId, TPrincipalId>> _events = new();
    protected AuditableAggregateRoot(TId id, TPrincipalId principalId) : base(id, principalId)
    {
        TypeId = AggregateTypeIdProvider.Get(this);
    }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _events.ToImmutableArray();

    public string TypeId { get; private set; }

    public void ClearEvents()
    {
        _events.Clear();
    }

    protected void AddEvent(IAuditableDomainEvent<TId, TPrincipalId> @event)
    {
        if (null == @event)
            throw new ArgumentNullException(nameof(@event));

        _events.Enqueue(@event);
    }
}
