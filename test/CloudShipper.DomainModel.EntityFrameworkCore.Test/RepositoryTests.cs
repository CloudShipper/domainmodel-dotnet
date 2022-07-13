using CloudShipper.DomainModel.Aggregate;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Extensions;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Infrastructure;
using CloudShipper.DomainModel.Infrastructure;
using CloudShipper.DomainModel.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test;

[Collection("C_006")]
[TestCaseOrderer(AlphabeticalTestCaseOrderer.TypeName, AlphabeticalTestCaseOrderer.AssemblyName)]
public class RepositoryTests : IClassFixture<ServiceConfigurationTestFixture>
{
    private readonly ServiceConfigurationTestFixture _fixture;

    public RepositoryTests(ServiceConfigurationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Test_001_Aggregate_AddAndGetById()
    {
        var id = Guid.NewGuid();
        var repo = _fixture.ServiceProvider.GetRequiredService<IAggregateRootRepository<DomainObjectA, Guid>>();
        var factory = _fixture.ServiceProvider.GetRequiredService<IAggregateRootFactory<DomainObjectA, Guid>>();

        var aggregate = factory.Create(id);
        aggregate.SetValue1(42);
        aggregate = repo.AddAsync(aggregate).Result;
        Assert.NotNull(aggregate);

        var result = repo.GetAsync(id).Result;
        Assert.NotNull(result);

        Assert.Equal(aggregate, result);
    }

    [Fact]
    public void Test_002_Aggregate_GetAll()
    {
        var repo = _fixture.ServiceProvider.GetRequiredService<IAggregateRootRepository<DomainObjectA, Guid>>();
        var factory = _fixture.ServiceProvider.GetRequiredService<IAggregateRootFactory<DomainObjectA, Guid>>();

        var ids = new HashSet<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        foreach (var id in ids)
        {
            var aggregate = factory.Create(id);
            aggregate = repo.AddAsync(aggregate).Result;
            Assert.NotNull(aggregate);
        }

        var result = repo.GetAllAsync().Result;

        ids.Union(result.Select(a => a.Id));
        Assert.Equal(2, ids.Count);
    }

    [Fact]
    public void Test_003_Aggregate_DeleteById()
    {
        var id = Guid.NewGuid();
        var repo = _fixture.ServiceProvider.GetRequiredService<IAggregateRootRepository<DomainObjectA, Guid>>();
        var factory = _fixture.ServiceProvider.GetRequiredService<IAggregateRootFactory<DomainObjectA, Guid>>();

        var aggregate = factory.Create(id);
        aggregate.SetValue1(42);
        aggregate = repo.AddAsync(aggregate).Result;
        Assert.NotNull(aggregate);

        var result = repo.DeleteAsync(id).Result;
        Assert.Equal(aggregate, result);

        Assert.ThrowsAsync<KeyNotFoundException>(() => repo.GetAsync(id));

        Assert.ThrowsAsync<KeyNotFoundException>(() => repo.DeleteAsync(id));
    }

    [Fact]
    public void Test_003_Aggregate_DeleteByInstance()
    {
        var id = Guid.NewGuid();
        var repo = _fixture.ServiceProvider.GetRequiredService<IAggregateRootRepository<DomainObjectA, Guid>>();
        var factory = _fixture.ServiceProvider.GetRequiredService<IAggregateRootFactory<DomainObjectA, Guid>>();

        var aggregate = factory.Create(id);
        aggregate.SetValue1(42);
        aggregate = repo.AddAsync(aggregate).Result;
        Assert.NotNull(aggregate);

        var result = repo.DeleteAsync(aggregate).Result;
        Assert.Equal(aggregate, result);

        Assert.ThrowsAsync<KeyNotFoundException>(() => repo.GetAsync(id));
    }

    [Fact]
    public void Test_004_Aggregate_Save()
    {
        var id = Guid.NewGuid();
        var repo = _fixture.ServiceProvider.GetRequiredService<IAggregateRootRepository<DomainObjectA, Guid>>();
        var factory = _fixture.ServiceProvider.GetRequiredService<IAggregateRootFactory<DomainObjectA, Guid>>();

        var aggregate = factory.Create(id);
        aggregate.SetValue1(42);
        aggregate = repo.AddAsync(aggregate).Result;
        Assert.NotNull(aggregate);

        aggregate.SetValue1(0);
        repo.SaveAsync(aggregate).Wait();

        var result = repo.GetAsync(id).Result;
        Assert.NotNull(result);
        Assert.Equal(0, result.Value1);
    }

    [Fact]
    public void Test_005_Aggregate_ThrowArgumentNullException()
    {
        var repo = _fixture.ServiceProvider.GetRequiredService<IAggregateRootRepository<DomainObjectA, Guid>>();

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.ThrowsAsync<ArgumentNullException>(() => repo.AddAsync(null));
        Assert.ThrowsAsync<ArgumentNullException>(() => repo.DeleteAsync(null));
        Assert.ThrowsAsync<ArgumentNullException>(() => repo.SaveAsync(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type
    }

    [Fact]
    public void Test_006_Aggregate_Transaction()
    {
        var unitOfWork = _fixture.ServiceProvider.GetRequiredService<IUnitOfWork<TestDbContext>>();
        var transactionProvider = (ITransactionProvider)unitOfWork;
        Assert.NotNull(transactionProvider);

        var id = Guid.NewGuid();
        var repo = _fixture.ServiceProvider.GetRequiredService<IAggregateRootRepository<DomainObjectA, Guid>>();
        var factory = _fixture.ServiceProvider.GetRequiredService<IAggregateRootFactory<DomainObjectA, Guid>>();

        using (var tx = transactionProvider.BeginTransactionAsync().Result)
        {
            var aggregate = factory.Create(id);
            aggregate.SetValue1(42);
            aggregate = repo.AddAsync(aggregate).Result;
            Assert.NotNull(aggregate);

            tx.CommitAsync().Wait();
        }
    }

    [Fact]
    public void Test_007_AuditableAggregate_AddAndGetById()
    {
        var id = Guid.NewGuid();
        var principalId = Guid.NewGuid();
        var repo = _fixture.ServiceProvider.GetRequiredService<IAuditableAggregateRootRepository<AuditableDomainObjectA, Guid, Guid>>();
        var factory = _fixture.ServiceProvider.GetRequiredService<IAuditableAggregateRootFactory<AuditableDomainObjectA, Guid, Guid>>();

        var aggregate = factory.Create(id, principalId);
        aggregate.SetValue1(42, principalId);
        aggregate = repo.AddAsync(aggregate).Result;
        Assert.NotNull(aggregate);

        var result = repo.GetAsync(id).Result;
        Assert.NotNull(result);

        Assert.Equal(aggregate, result);
    }

    [Fact]
    public void Test_008_AuditableAggregate_GetAll()
    {
        var repo = _fixture.ServiceProvider.GetRequiredService<IAuditableAggregateRootRepository<AuditableDomainObjectA, Guid, Guid>>();
        var factory = _fixture.ServiceProvider.GetRequiredService<IAuditableAggregateRootFactory<AuditableDomainObjectA, Guid, Guid>>();

        var ids = new HashSet<Tuple<Guid, Guid>> 
        { 
            Tuple.Create(Guid.NewGuid(), Guid.NewGuid()),
            Tuple.Create(Guid.NewGuid(), Guid.NewGuid()),
        };

        foreach (var id in ids)
        {
            var aggregate = factory.Create(id.Item1, id.Item2);
            aggregate = repo.AddAsync(aggregate).Result;
            Assert.NotNull(aggregate);
        }

        var result = repo.GetAllAsync().Result;

        ids.Union(result.Select(a => Tuple.Create(a.Id, a.CreatedBy)));
        Assert.Equal(2, ids.Count);
    }

    [Fact]
    public void Test_009_AuditableAggregate_DeleteById()
    {
        var id = Guid.NewGuid();
        var principalId = Guid.NewGuid();
        var repo = _fixture.ServiceProvider.GetRequiredService<IAuditableAggregateRootRepository<AuditableDomainObjectA, Guid, Guid>>();
        var factory = _fixture.ServiceProvider.GetRequiredService<IAuditableAggregateRootFactory<AuditableDomainObjectA, Guid, Guid>>();

        var aggregate = factory.Create(id, principalId);
        aggregate.SetValue1(42, principalId);
        aggregate = repo.AddAsync(aggregate).Result;
        Assert.NotNull(aggregate);

        var result = repo.DeleteAsync(id).Result;
        Assert.Equal(aggregate, result);

        Assert.ThrowsAsync<KeyNotFoundException>(() => repo.GetAsync(id));

        Assert.ThrowsAsync<KeyNotFoundException>(() => repo.DeleteAsync(id));
    }

    [Fact]
    public void Test_010_AuditableAggregate_DeleteByInstance()
    {
        var id = Guid.NewGuid();
        var principalId = Guid.NewGuid();
        var repo = _fixture.ServiceProvider.GetRequiredService<IAuditableAggregateRootRepository<AuditableDomainObjectA, Guid, Guid>>();
        var factory = _fixture.ServiceProvider.GetRequiredService<IAuditableAggregateRootFactory<AuditableDomainObjectA, Guid, Guid>>();

        var aggregate = factory.Create(id, principalId);
        aggregate.SetValue1(42, principalId);
        aggregate = repo.AddAsync(aggregate).Result;
        Assert.NotNull(aggregate);

        var result = repo.DeleteAsync(aggregate).Result;
        Assert.Equal(aggregate, result);

        Assert.ThrowsAsync<KeyNotFoundException>(() => repo.GetAsync(id));
    }

    [Fact]
    public void Test_011_AuditableAggregate_Save()
    {
        var id = Guid.NewGuid();
        var principalId = Guid.NewGuid();
        var repo = _fixture.ServiceProvider.GetRequiredService<IAuditableAggregateRootRepository<AuditableDomainObjectA, Guid, Guid>>();
        var factory = _fixture.ServiceProvider.GetRequiredService<IAuditableAggregateRootFactory<AuditableDomainObjectA, Guid, Guid>>();

        var aggregate = factory.Create(id, principalId);
        aggregate.SetValue1(42, principalId);
        aggregate = repo.AddAsync(aggregate).Result;
        Assert.NotNull(aggregate);

        aggregate.SetValue1(0, principalId);
        repo.SaveAsync(aggregate).Wait();

        var result = repo.GetAsync(id).Result;
        Assert.NotNull(result);
        Assert.Equal(0, result.Value1);
    }

    [Fact]
    public void Test_012_AuditableAggregate_ThrowArgumentNullException()
    {
        var repo = _fixture.ServiceProvider.GetRequiredService<IAuditableAggregateRootRepository<AuditableDomainObjectA, Guid, Guid>>();

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.ThrowsAsync<ArgumentNullException>(() => repo.AddAsync(null));
        Assert.ThrowsAsync<ArgumentNullException>(() => repo.DeleteAsync(null));
        Assert.ThrowsAsync<ArgumentNullException>(() => repo.SaveAsync(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type
    }
}
