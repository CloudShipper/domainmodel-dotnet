using CloudShipper.DomainModel.Events;
using CloudShipper.DomainModel.Test.Domain;
using CloudShipper.DomainModel.Test.Extensions;

namespace CloudShipper.DomainModel.Test;

[Collection("C_004")]
[TestCaseOrderer(AlphabeticalTestCaseOrderer.TypeName, AlphabeticalTestCaseOrderer.AssemblyName)]
public class DomainEventTypeIdProviderTest
{
    [Fact]
    public void Test_001_GetTypeIdByType()
    {
        AggregateTypeIdProvider.ReadAllTypes(new[] { typeof(DomainObjectA) });
        DomainEventTypeIdProvider.ReadAllTypes(new[] { typeof(DomainObjectA) });

        var obj = new DomainObjectA(Guid.NewGuid());
        var evt = new DomainEventA(obj.Id, obj.GetType().Name, obj.TypeId);

        var typeId = DomainEventTypeIdProvider.Get(evt);
        Assert.NotNull(typeId);
        Assert.Equal(Constants.DomainEventA, typeId);

        typeId = DomainEventTypeIdProvider.Get(evt.GetType());
        Assert.Equal(Constants.DomainEventA, typeId);

        var aObj = new AuditableDomainObjectA(Guid.NewGuid(), Guid.Parse(Constants.PrincipalIdA));
        var aEvt = new AuditableDomainEventA(aObj.Id, aObj.GetType().Name, aObj.TypeId, Guid.Parse(Constants.PrincipalIdA));

        typeId = DomainEventTypeIdProvider.Get(aEvt);
        Assert.NotNull(typeId);
        Assert.Equal(Constants.DomainEventA, typeId);
    }

    [Fact]
    public void Test_002_GetTypeByTypeId()
    {
        DomainEventTypeIdProvider.ReadAllTypes(new[] { typeof(DomainObjectA) });
        var type = DomainEventTypeIdProvider.Get(Constants.DomainEventA);
        Assert.Equal(typeof(DomainEventA), type);
    }

    [Fact]
    public void Test_003_NoDomainEvent()
    {
        DomainEventTypeIdProvider.ReadAllTypes(new[] { typeof(DomainObjectA) });
        Assert.Throws<KeyNotFoundException>(() => DomainEventTypeIdProvider.Get(typeof(NoDomainEvent)));
    }

    [Fact]
    public void Test_004_InvalidTypeId()
    {
        DomainEventTypeIdProvider.ReadAllTypes(new[] { typeof(DomainObjectA) });
        Assert.Throws<KeyNotFoundException>(() => DomainEventTypeIdProvider.Get(String.Empty));
    }

    [Fact]
    public void Test_005_NoDomainEventAttribute()
    {
        DomainEventTypeIdProvider.ReadAllTypes(new[] { typeof(DomainObjectA) });
        Assert.Throws<KeyNotFoundException>(() => new NoDomainEventAttribute(Guid.NewGuid(),
                                                                             typeof(DomainObjectA).GetType().Name,
                                                                             Constants.DomainObjectATypeId));
    }

    [Fact]
    public void Test_006_DomainEventIsNull()
    {
        DomainEventTypeIdProvider.ReadAllTypes(new[] { typeof(DomainObjectA) });
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        NoDomainEventAttribute evt = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
        Assert.Throws<ArgumentNullException>(() => DomainEventTypeIdProvider.Get(evt));
#pragma warning restore CS8604 // Possible null reference argument.
    }
}
