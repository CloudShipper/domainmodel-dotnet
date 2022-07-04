using CloudShipper.DomainModel.Test.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudShipper.DomainModel.Test;

[Collection("C_001")]
public class AggregateTest
{
    [Fact]
    public void Test_001_AggregateRootProperties()
    {
        AggregateTypeIdProvider.ReadAllTypes(new[] { typeof(DomainObjectA) });

        var id = Guid.NewGuid();
        var obj = new DomainObjectA(id);

        Assert.Equal(Constants.DomainObjectATypeId, obj.TypeId);
        Assert.Equal(id, obj.Id);
        Assert.NotNull(obj.DomainEvents);
        Assert.Equal(0, obj.DomainEvents.Count);
    }

    [Fact]
    public void Test_002_AuditableAggregateRootProperties()
    {
        AggregateTypeIdProvider.ReadAllTypes(new[] { typeof(AuditableDomainObjectA) });

        var id = Guid.NewGuid();
        var principalId = Guid.NewGuid();
        var obj = new AuditableDomainObjectA(id, principalId);

        Assert.Equal(Constants.AuditableDomainObjectATypeId, obj.TypeId);
        Assert.Equal(id, obj.Id);
        Assert.Equal(principalId, obj.CreatedBy);
        Assert.NotNull(obj.CreatedAt);
        Assert.Equal(Guid.Empty, obj.ModifiedBy);
        Assert.Null(obj.ModifiedAt);
        Assert.Equal(Guid.Empty, obj.DeletedBy);
        Assert.Null(obj.DeletedAt);
        Assert.NotNull(obj.AuditableDomainEvents);
        Assert.Equal(0, obj.AuditableDomainEvents.Count);
    }

    [Fact]
    public void Test_003_AggregateRootEvent()
    {
        AggregateTypeIdProvider.ReadAllTypes(new[] { typeof(AuditableDomainObjectA) });

        var obj = new DomainObjectA(Guid.NewGuid());
        Assert.Throws<ArgumentNullException>(() => obj.AddNullTestEvent());

        obj.AddTestEvent();
        Assert.Equal(1, obj.DomainEvents.Count);
        obj.ClearEvents();
        Assert.Equal(0, obj.DomainEvents.Count);
    }

    [Fact]
    public void Test_004_AuditableAggregateRootEvent()
    {
        AggregateTypeIdProvider.ReadAllTypes(new[] { typeof(AuditableDomainObjectA) });

        var obj = new AuditableDomainObjectA(Guid.NewGuid(), Guid.NewGuid());
        Assert.Throws<ArgumentNullException>(() => obj.AddNullTestEvent());

        obj.AddTestEvent();
        Assert.Equal(1, obj.AuditableDomainEvents.Count);
        obj.ClearEvents();
        Assert.Equal(0, obj.AuditableDomainEvents.Count);
    }
}
