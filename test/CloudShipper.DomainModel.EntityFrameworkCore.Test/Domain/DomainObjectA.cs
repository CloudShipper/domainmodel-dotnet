using CloudShipper.DomainModel.Aggregate;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain;

[Aggregate("7934B10B-9575-4966-A477-8DD1F2BCE140")]
internal class DomainObjectA : AggregateRoot<Guid>
{
    public DomainObjectA(Guid id) : base(id)
    {
    }

    public int Value1 { get; set; } = 1;
}
