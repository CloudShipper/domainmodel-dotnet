using CloudShipper.DomainModel.Test.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace CloudShipper.DomainModel.Test;

public class ServiceConfigurationTestFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; set; }

    public ServiceConfigurationTestFixture()
    {
        var services = new ServiceCollection();
        services.AddDomain(new[] { typeof(DomainObjectA) });
        ServiceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        ServiceProvider.Dispose();
    }
}
