using CloudShipper.DomainModel.EntityFrameworkCore.Infrastructure;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Extensions;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Infrastructure;
using CloudShipper.DomainModel.Infrastructure;
using CloudShipper.DomainModel.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test
{
    [Collection("C_004")]
    [TestCaseOrderer(AlphabeticalTestCaseOrderer.TypeName, AlphabeticalTestCaseOrderer.AssemblyName)]
    public class ServiceConfigurationTest :IClassFixture<ServiceConfigurationTestFixture>
    {
        private readonly ServiceConfigurationTestFixture _fixture;

        public ServiceConfigurationTest(ServiceConfigurationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Test_001_GetIUnitOfWork()
        {
            var unitOfWork = _fixture.ServiceProvider.GetRequiredService<IUnitOfWork<TestDbContext>>();
            Assert.NotNull(unitOfWork);            
        }

        [Fact]
        public void Test_002_GetAggregateRootRepository()
        {
            // Check null and all repos must use the same IUnitOfWork instance !!
            var unitOfWork = _fixture.ServiceProvider.GetRequiredService<IUnitOfWork<TestDbContext>>();
            Assert.NotNull(unitOfWork);

            var repoA1 = _fixture.ServiceProvider.GetRequiredService<IAggregateRootRepository<DomainObjectA, Guid>>();
            Assert.NotNull(repoA1);
            Assert.Same(unitOfWork, ((AggregateRootRepository<TestDbContext, DomainObjectA, Guid>)repoA1).UnitOfWork);

            var repoA2 = _fixture.ServiceProvider.GetRequiredService<IAggregateRootRepository<DomainObjectA, Guid>>();
            Assert.NotNull(repoA2);

            Assert.Same(repoA1, repoA2);
            Assert.Same(unitOfWork, ((AggregateRootRepository<TestDbContext, DomainObjectA, Guid>)repoA2).UnitOfWork);

            var repoB1 = _fixture.ServiceProvider.GetRequiredService<IAggregateRootRepository<DomainObjectB, Guid>>();
            Assert.NotNull(repoB1);

            Assert.Same(unitOfWork, ((AggregateRootRepository<TestDbContext, DomainObjectB, Guid>)repoB1).UnitOfWork);
        }

        [Fact]
        public void Test_003_GetAuditableAggregateRootRepository()
        {
            var unitOfWork = _fixture.ServiceProvider.GetRequiredService<IUnitOfWork<TestDbContext>>();
            Assert.NotNull(unitOfWork);

            var repo = _fixture.ServiceProvider.GetRequiredService<IAuditableAggregateRootRepository<AuditableDomainObjectA, Guid, Guid>>();
            Assert.NotNull(repo);

            Assert.Same(unitOfWork, ((AuditableAggregateRootRepository<TestDbContext, AuditableDomainObjectA, Guid, Guid>)repo).UnitOfWork);
        }

        [Fact]
        public void Test_004_ScopedLifetime()
        {
            var defaultUnitOfWork = _fixture.ServiceProvider.GetRequiredService<IUnitOfWork<TestDbContext>>();
            Assert.NotNull(defaultUnitOfWork);

            var defaultRepo = _fixture.ServiceProvider.GetRequiredService<IAggregateRootRepository<DomainObjectA, Guid>>();
            Assert.NotNull(defaultRepo);

            var defaultAuditableRepo = _fixture.ServiceProvider.GetRequiredService<IAuditableAggregateRootRepository<AuditableDomainObjectA, Guid, Guid>>();
            Assert.NotNull(defaultAuditableRepo);

            using (var scope = _fixture.ServiceProvider.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork<TestDbContext>>();
                Assert.NotNull(unitOfWork);

                Assert.NotSame(defaultUnitOfWork, unitOfWork);

                var repo = scope.ServiceProvider.GetRequiredService<IAggregateRootRepository<DomainObjectA, Guid>>();
                Assert.NotNull(repo);

                var auditableRepo = scope.ServiceProvider.GetRequiredService<IAuditableAggregateRootRepository<AuditableDomainObjectA, Guid, Guid>>();
                Assert.NotNull(auditableRepo);

                Assert.NotSame(defaultRepo, repo);
                Assert.NotSame(defaultAuditableRepo, auditableRepo);
            }
        }
    }
}
