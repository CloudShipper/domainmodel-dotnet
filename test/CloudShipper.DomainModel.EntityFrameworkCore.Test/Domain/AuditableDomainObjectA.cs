using CloudShipper.DomainModel.Aggregate;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain
{
    [Aggregate("F48DBCE9-EC33-4917-B2D4-3FB46146C12A")]
    internal class AuditableDomainObjectA : AuditableAggregateRoot<Guid, Guid>
    {
        private AuditableDomainObjectA()
            : base(Guid.Empty, Guid.Empty)
        {

        }

        public AuditableDomainObjectA(Guid id, Guid principalId) : base(id, principalId)
        {
        }

        public int Value1 { get; set; }
    }
}
