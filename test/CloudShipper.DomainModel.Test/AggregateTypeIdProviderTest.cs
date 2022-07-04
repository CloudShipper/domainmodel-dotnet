using CloudShipper.DomainModel.Test.Domain;
using CloudShipper.DomainModel.Test.Extensions;

namespace CloudShipper.DomainModel.Test;

[Collection("C_002")]
[TestCaseOrderer(AlphabeticalTestCaseOrderer.TypeName, AlphabeticalTestCaseOrderer.AssemblyName)]
public class AggregateTypeIdProviderTest
{
    [Fact]
    public void Test_001_GetTypeIdByType()
    {
        AggregateTypeIdProvider.ReadAllTypes(new[] { typeof(DomainObjectA) });

        var obj = new DomainObjectA(Guid.NewGuid());
        var typeId = AggregateTypeIdProvider.Get(obj);
        Assert.NotNull(typeId);
        Assert.Equal(Constants.DomainObjectATypeId, typeId);
        typeId = AggregateTypeIdProvider.Get(typeof(DomainObjectA));
        Assert.NotNull(typeId);
        Assert.Equal(Constants.DomainObjectATypeId, typeId);

        var type = AggregateTypeIdProvider.Get(typeId);
        Assert.NotNull(type);
        Assert.Equal(typeof(DomainObjectA), type);

        var auditableObj = new AuditableDomainObjectA(Guid.NewGuid(), Guid.Parse(Constants.PrincipalIdA));
        typeId = AggregateTypeIdProvider.Get(auditableObj);
        Assert.NotNull(typeId);
        Assert.Equal(Constants.AuditableDomainObjectATypeId, typeId);
        typeId = AggregateTypeIdProvider.Get(typeof(AuditableDomainObjectA));
        Assert.NotNull(typeId);
        Assert.Equal(Constants.AuditableDomainObjectATypeId, typeId);

        type = AggregateTypeIdProvider.Get(typeId);
        Assert.NotNull(type);
        Assert.Equal(typeof(AuditableDomainObjectA), type);
    }
}