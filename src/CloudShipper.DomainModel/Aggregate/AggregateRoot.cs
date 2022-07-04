using CloudShipper.DomainModel.Entity;
using CloudShipper.DomainModel.Events;
using System.Collections.Immutable;

namespace CloudShipper.DomainModel.Aggregate;

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot<TId>
{
    private readonly Queue<IDomainEvent<TId>> _events = new();

    protected AggregateRoot(TId id) : base(id)
    {
        TypeId = AggregateTypeIdProvider.Get(this);
    }

    public IReadOnlyCollection<IDomainEvent<TId>> DomainEvents => _events.ToImmutableArray();

    public string TypeId { get; private set; }

    public void ClearEvents()
    {
        _events.Clear();
    }

    protected void AddEvent(IDomainEvent<TId> @event)
    {
        if (null == @event)
            throw new ArgumentNullException(nameof(@event));

        _events.Enqueue(@event);
    }
}
