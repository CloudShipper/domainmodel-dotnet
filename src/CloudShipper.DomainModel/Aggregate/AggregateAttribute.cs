namespace CloudShipper.DomainModel.Aggregate;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class AggregateAttribute : Attribute
{
    public AggregateAttribute(string aggregateTypeId)
    {
        AggregateTypeId = aggregateTypeId;
    }

    public string AggregateTypeId { get; private set; }
}
