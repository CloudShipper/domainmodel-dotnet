using CloudShipper.DomainModel.Aggregate;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Extensions;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Infrastructure;
using CloudShipper.DomainModel.Infrastructure;
using CloudShipper.DomainModel.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test;

[Collection("C_007")]
[TestCaseOrderer(AlphabeticalTestCaseOrderer.TypeName, AlphabeticalTestCaseOrderer.AssemblyName)]
public class ResilientTransactionTest : IClassFixture<ServiceConfigurationTestFixture>
{
    private readonly ServiceConfigurationTestFixture _fixture;

    public ResilientTransactionTest(ServiceConfigurationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Test_001_Commit()
    {
        var id = Guid.NewGuid();
        var factory = _fixture.ServiceProvider.GetRequiredService<IAggregateRootFactory<DomainObjectA, Guid>>();
        var repository = _fixture.ServiceProvider.GetRequiredService<IAggregateRootRepository<DomainObjectA, Guid>>();
        var unitOfWork = _fixture.ServiceProvider.GetRequiredService<IUnitOfWork<TestDbContext>>();
        var transactionProvider = (ITransactionable)unitOfWork;

        var resilientTransaction = transactionProvider.NewResilientTransaction();
        Assert.NotNull(resilientTransaction);

        resilientTransaction.ExecuteAsync(() =>
        {
            var obj = factory.Create(id);
            repository.AddAsync(obj).Wait();
            return Task.CompletedTask;
        }).Wait();

        var result = repository.GetAsync(id).Result;
        Assert.NotNull(result);
    }
}
