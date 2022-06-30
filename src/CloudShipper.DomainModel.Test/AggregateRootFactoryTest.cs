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
}
