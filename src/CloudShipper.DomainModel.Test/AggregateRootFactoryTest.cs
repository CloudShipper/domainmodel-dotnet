using CloudShipper.DomainModel.Test.Domain;

namespace CloudShipper.DomainModel.Test;

public class AggregateRootFactoryTest
{
    [Fact]
    public void Test_001_CreateInstanceWithId()
    {
        AggregateTypeIdProvider.ReadAllTypes(new[] { typeof(DomainObjectA) });

        var factory = new DomainObjectAFactory();
        var aggregate = factory.Create(Guid.NewGuid());

        Assert.NotNull(aggregate);
        Assert.Equal(Constants.DomainObjectATypeId, aggregate.TypeId);
        Assert.NotNull(aggregate.DomainEvents);
    }

    [Fact]
    public void Test_001_CreateAuditableInstanceWithId()
    {
        AggregateTypeIdProvider.ReadAllTypes(new[] { typeof(AuditableDomainObjectA) });

        var factory = new AuditableDomainObjectAFactory();
        var aggregate = factory.Create(Guid.NewGuid(), Guid.Parse(Constants.PrincipalIdA));
        Assert.NotNull(aggregate);
        Assert.Equal(Constants.AuditableDomainObjectATypeId, aggregate.TypeId);
        Assert.Equal(Guid.Parse(Constants.PrincipalIdA), aggregate.CreatedBy);
    }
}
