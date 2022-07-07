using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Infrastructure;
using CloudShipper.DomainModel.Events;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test;

public class ServiceConfigurationTestFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; set; }
    public Mock<IDomainEventDispatcher> DomainEventDispatcherMock { get; set; }

    public ServiceConfigurationTestFixture()
    {
        var dispatcher = new Mock<IDomainEventDispatcher>();
        DomainEventDispatcherMock = dispatcher;

        var services = new ServiceCollection();
        services
            .AddDbContext<TestDbContext>()
            .AddDomain(new[] { typeof(DomainObjectA) })
            .AddSingleton(dispatcher.Object)
            .AddUnitOfWork<TestDbContext>(binder => binder
                .Bind<DomainObjectA, Guid>()
                .Bind<DomainObjectB, Guid>())
            .AddEfCoreInfrastructure(new[] { typeof(DomainObjectA)});

        ServiceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        ServiceProvider.Dispose();
    }
}
