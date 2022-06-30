namespace CloudShipper.DomainModel.Events;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DomainEventAttribute : Attribute
{
    public DomainEventAttribute(string eventTypeId)
    {
        EventTypeId = eventTypeId;
    }

    public string EventTypeId { get; private set; }
}
