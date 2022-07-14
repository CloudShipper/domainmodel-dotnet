using CloudShipper.DomainModel.Aggregate;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain.Events.AuditableDomainObjectA;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain
{
    [Aggregate("F48DBCE9-EC33-4917-B2D4-3FB46146C12A")]
    internal class AuditableDomainObjectA : AuditableAggregateRoot<Guid, Guid>
    {
        // needed by EFCore
        private AuditableDomainObjectA()
            : base(Guid.Empty, Guid.Empty)
        {

        }

        public AuditableDomainObjectA(Guid id, Guid principalId) : base(id, principalId)
        {
            AddEvent(new CreatedEvent(id, principalId));
        }

        public int Value1 { get; private set; }

        public void SetValue1(int value, Guid principalId)
        {
            Value1 = value;
            var evt = new Value1ChangedEvent(Id, principalId) { Value1 = value };
            ModifiedBy = principalId;
            ModifiedAt = evt.Timestamp;
            AddEvent(evt);
        }
    }
}
