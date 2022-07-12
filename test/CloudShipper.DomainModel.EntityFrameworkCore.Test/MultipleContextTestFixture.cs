using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Infrastructure;
using CloudShipper.DomainModel.Events;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test;

public class MultipleContextTestFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; set; }
    public Mock<IDomainEventDispatcher> DomainEventDispatcherMock { get; set; }
    
    private readonly SqliteConnection _sqliteConnection;

    public MultipleContextTestFixture()
    {
        _sqliteConnection = new SqliteConnection("DataSource=:memory:");
        _sqliteConnection.Open();

        var dispatcher = new Mock<IDomainEventDispatcher>();
        DomainEventDispatcherMock = dispatcher;

        var services = new ServiceCollection();
        services
            .AddDbContext<AnotherTestDbContext>(options => options.UseSqlite(_sqliteConnection))
            .AddDbContext<YetAnotherTestDbContext>(options => options.UseSqlite(_sqliteConnection))
            .AddDomain(new[] { typeof(DomainObjectA) })
            .AddSingleton(dispatcher.Object)
            .AddRepositories<AnotherTestDbContext>(binder => binder
                .Bind<DomainObjectA, Guid>())
            .AddRepositories<YetAnotherTestDbContext>(binder => binder
                .Bind<DomainObjectB, Guid>())
            .AddEfCoreInfrastructure(new[] { typeof(DomainObjectA) });

        ServiceProvider = services.BuildServiceProvider();

        var contextOne = ServiceProvider.GetRequiredService<AnotherTestDbContext>();;
        var contextTwo = ServiceProvider.GetRequiredService<YetAnotherTestDbContext>();

        contextOne.Database.GetService<IRelationalDatabaseCreator>().CreateTables();
        contextTwo.Database.GetService<IRelationalDatabaseCreator>().CreateTables();
    }

    public void Dispose()
    {
        _sqliteConnection.Close();
        ServiceProvider.Dispose();
    }
}
