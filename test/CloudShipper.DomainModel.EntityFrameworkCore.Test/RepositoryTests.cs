﻿using CloudShipper.DomainModel.Aggregate;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Extensions;
using CloudShipper.DomainModel.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
    public void Test_001_AddAndGetById()
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
    public void Test_002_GetAll()
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
    public void Test_003_DeleteById()
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

        Assert.ThrowsAsync<KeyNotFoundException>(() => repo.GetAsync(Guid.NewGuid()));
    }

    [Fact]
    public void Test_003_DeleteByAggregate()
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
    public void Test_004_Save()
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
    public void Test_005_ThrowArgumentNullException()
    {
        var repo = _fixture.ServiceProvider.GetRequiredService<IAggregateRootRepository<DomainObjectA, Guid>>();

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.ThrowsAsync<ArgumentNullException>(() => repo.AddAsync(null));
        Assert.ThrowsAsync<ArgumentNullException>(() => repo.DeleteAsync(null));
        Assert.ThrowsAsync<ArgumentNullException>(() => repo.SaveAsync(null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type
    }
}
