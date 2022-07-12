using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Infrastructure;
using CloudShipper.DomainModel.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test;

public class ServiceConfigurationTestFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; set; }
    public Mock<IDomainEventDispatcher> DomainEventDispatcherMock { get; set; }

    private TestDbContext _context;

    public ServiceConfigurationTestFixture()
    {
        var dispatcher = new Mock<IDomainEventDispatcher>();
        DomainEventDispatcherMock = dispatcher;

        var services = new ServiceCollection();
        services
            .AddDbContext<TestDbContext>()
            .AddDomain(new[] { typeof(DomainObjectA) })
            .AddSingleton(dispatcher.Object)
            .AddRepositories<TestDbContext>(binder => binder
                .Bind<DomainObjectA, Guid>()
                .Bind<DomainObjectB, Guid>()
                .Bind<AuditableDomainObjectA, Guid, Guid>())
            .AddEfCoreInfrastructure(new[] { typeof(DomainObjectA)});

        ServiceProvider = services.BuildServiceProvider();
        _context = ServiceProvider.GetRequiredService<TestDbContext>();
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _context.Database.CloseConnection();
        ServiceProvider.Dispose();
    }
}
