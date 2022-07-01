using CloudShipper.DomainModel.Test.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace CloudShipper.DomainModel.Test;

public class ServiceConfigurationTest : IClassFixture<ServiceConfigurationTestFixture>
{
    private readonly ServiceConfigurationTestFixture _fixture;

    public ServiceConfigurationTest(ServiceConfigurationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Test_001_Factories()
    {
        // AggregateRoot
        var factoryA = _fixture.ServiceProvider.GetRequiredService<IAggregateRootFactory<DomainObjectA, Guid>>();
        Assert.NotNull(factoryA);

        var factoryB = _fixture.ServiceProvider.GetRequiredService<IAggregateRootFactory<DomainObjectB, Guid>>();
        Assert.NotNull(factoryB);

        var factoryBConcrete = _fixture.ServiceProvider.GetRequiredService<IDomainObjectBFactory>();
        Assert.NotNull(factoryBConcrete);

        var aB = factoryBConcrete.Create();
        Assert.NotNull(aB);

        // it doesn't depend on which interface is queried, we expect that same instance is the result !!
        Assert.Same(factoryB, factoryBConcrete);

        // AuditableAggregateRoot
        var auditableFactoryA = _fixture.ServiceProvider.GetRequiredService<IAuditableAggregateRootFactory<AuditableDomainObjectA, Guid, Guid>>();
        Assert.NotNull(auditableFactoryA);

        var auditableA = auditableFactoryA.Create(Guid.NewGuid(), Guid.Parse(Constants.PrincipalIdA));
        Assert.NotNull(auditableA);

        var auditableFactoryB = _fixture.ServiceProvider.GetRequiredService<IAuditableAggregateRootFactory<AuditableDomainObjectB, Guid, Guid>>();
        Assert.NotNull(auditableFactoryB);

        var concretAuditBFactory = _fixture.ServiceProvider.GetRequiredService<IAuditableDomainObjectBFactory>();
        Assert.NotNull(concretAuditBFactory);

        // it doesn't depend on which interface is queried, we expect that same instance is the result !!
        Assert.Same(auditableFactoryB, concretAuditBFactory);

        // AggregatRoot with no own factory implementation
        var factorySimple = _fixture.ServiceProvider.GetRequiredService<IAggregateRootFactory<SimpleDomainObject, Guid>>();
        Assert.NotNull(factorySimple);
    }
}
