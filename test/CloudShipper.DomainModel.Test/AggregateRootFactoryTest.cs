using CloudShipper.DomainModel.Test.Domain;
using CloudShipper.DomainModel.Test.Extensions;

namespace CloudShipper.DomainModel.Test;

[Collection("C_002")]
[TestCaseOrderer(AlphabeticalTestCaseOrderer.TypeName, AlphabeticalTestCaseOrderer.AssemblyName)]
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
    public void Test_002_CreateAuditableInstanceWithId()
    {
        AggregateTypeIdProvider.ReadAllTypes(new[] { typeof(AuditableDomainObjectA) });

        var factory = new AuditableDomainObjectAFactory();
        var aggregate = factory.Create(Guid.NewGuid(), Guid.Parse(Constants.PrincipalIdA));
        Assert.NotNull(aggregate);
        Assert.Equal(Constants.AuditableDomainObjectATypeId, aggregate.TypeId);
        Assert.Equal(Guid.Parse(Constants.PrincipalIdA), aggregate.CreatedBy);
    }
}
