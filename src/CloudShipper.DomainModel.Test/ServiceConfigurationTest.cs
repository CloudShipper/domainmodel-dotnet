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
        var factoryA = _fixture.ServiceProvider.GetRequiredService<IAggregateRootFactory<DomainObjectA, Guid>>();
        Assert.NotNull(factoryA);

        var factoryB = _fixture.ServiceProvider.GetRequiredService<IAggregateRootFactory<DomainObjectB, Guid>>();
        Assert.NotNull(factoryB);

        var factoryBConcrete = _fixture.ServiceProvider.GetRequiredService<IDomainObjectBFactory>();
        Assert.NotNull(factoryBConcrete);

        var aB = factoryBConcrete.Create();
        Assert.NotNull(aB);        
    }
}
