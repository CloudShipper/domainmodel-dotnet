using CloudShipper.DomainModel.Aggregate;
using System.Security.Cryptography.X509Certificates;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain
{
    [Aggregate("F35FC974-E774-48E2-9ACB-FE3B05BD2E45")]
    internal class DomainObjectC : AggregateRoot<Guid>
    {
        public DomainObjectC(Guid id) : base(id)
        {           
        }
    }
}
