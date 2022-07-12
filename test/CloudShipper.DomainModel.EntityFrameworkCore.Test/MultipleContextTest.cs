using CloudShipper.DomainModel.Aggregate;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Extensions;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Infrastructure;
using CloudShipper.DomainModel.Infrastructure;
using CloudShipper.DomainModel.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test
{
    [Collection("C_008")]
    [TestCaseOrderer(AlphabeticalTestCaseOrderer.TypeName, AlphabeticalTestCaseOrderer.AssemblyName)]
    public class MultipleContextTest : IClassFixture<MultipleContextTestFixture>
    {
        private readonly MultipleContextTestFixture _fixture;

        public MultipleContextTest(MultipleContextTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Test_001_AddDomainObjects()
        {
            var idOne = Guid.NewGuid();
            var idTwo = Guid.NewGuid();

            var unitOfWorkOne = _fixture.ServiceProvider.GetRequiredService<IUnitOfWork<AnotherTestDbContext>>();
            var unitOfWorkTwo = _fixture.ServiceProvider.GetRequiredService<IUnitOfWork<YetAnotherTestDbContext>>();

            var factroyOne = _fixture.ServiceProvider.GetRequiredService<IAggregateRootFactory<DomainObjectA, Guid>>();
            var factoryTwo = _fixture.ServiceProvider.GetRequiredService<IAggregateRootFactory<DomainObjectB, Guid>>();

            var repoOne = _fixture.ServiceProvider.GetRequiredService<IAggregateRootRepository<DomainObjectA, Guid>>();
            var repoTwo = _fixture.ServiceProvider.GetRequiredService<IAggregateRootRepository<DomainObjectB, Guid>>();

            repoOne.AddAsync(factroyOne.Create(idOne)).Wait();
            repoTwo.AddAsync(factoryTwo.Create(idTwo)).Wait();

            var resultOne = repoOne.GetAsync(idOne).Result;
            var resultTwo = repoTwo.GetAsync(idTwo).Result;

            Assert.NotNull(resultOne);
            Assert.NotNull(resultTwo);
        }

        [Fact]
        public void Test_002_CrossContextTransaction()
        {
            var idOne = Guid.NewGuid();
            var idTwo = Guid.NewGuid();

            var unitOfWorkOne = _fixture.ServiceProvider.GetRequiredService<IUnitOfWork<AnotherTestDbContext>>();
            var unitOfWorkTwo = _fixture.ServiceProvider.GetRequiredService<IUnitOfWork<YetAnotherTestDbContext>>();

            var factroyOne = _fixture.ServiceProvider.GetRequiredService<IAggregateRootFactory<DomainObjectA, Guid>>();
            var factoryTwo = _fixture.ServiceProvider.GetRequiredService<IAggregateRootFactory<DomainObjectB, Guid>>();

            var repoOne = _fixture.ServiceProvider.GetRequiredService<IAggregateRootRepository<DomainObjectA, Guid>>();
            var repoTwo = _fixture.ServiceProvider.GetRequiredService<IAggregateRootRepository<DomainObjectB, Guid>>();

            var transactionProvider = (ITransactionable)unitOfWorkOne;
            var tx = transactionProvider.NewResilientTransaction();
            tx.ExecuteAsync(t => 
            {
                ((ITransactionable)unitOfWorkTwo).SaveChangesAsync(t).Wait();
                return Task.CompletedTask;
            }).Wait();
        }
    }
}
