namespace CloudShipper.DomainModel.Test.Domain;

[Aggregate(Constants.AuditableDomainObjectATypeId)]
internal class AuditableDomainObjectA : AuditableAggregateRoot<Guid, Guid>
{
    public AuditableDomainObjectA(Guid id, Guid principalId) : base(id, principalId)
    {
    }

    public void AddTestEvent()
    {
        base.AddEvent(new AuditableDomainEvent());
    }

    public void AddNullTestEvent()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        base.AddEvent(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }
}
