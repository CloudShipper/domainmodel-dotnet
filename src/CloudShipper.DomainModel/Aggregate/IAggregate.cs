using CloudShipper.DomainModel.Events;

namespace CloudShipper.DomainModel.Aggregate;

public interface IAggregate
{
    string TypeId { get; }
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearEvents();
}
