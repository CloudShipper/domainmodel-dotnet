using CloudShipper.DomainModel.Aggregate;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Extensions;
using CloudShipper.DomainModel.EntityFrameworkCore.Test.Infrastructure;
using CloudShipper.DomainModel.Events;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test;

[Collection("C_001")]
[TestCaseOrderer(AlphabeticalTestCaseOrderer.TypeName, AlphabeticalTestCaseOrderer.AssemblyName)]
public class DbContextExtensionsTest
{
    [Fact]
    public void Test_001_AddAndQueryAggregateRoot()
    {
        var id = Guid.NewGuid();

        AggregateTypeIdProvider.ReadAllTypes(new[] { typeof(DomainObjectA) });
        DomainEventTypeIdProvider.ReadAllTypes(new[] { typeof(DomainObjectA) });

        using var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        using (var context = new TestDbContext(connection))
        {            
            context.Database.EnsureCreated();
            var obj = new DomainObjectA(id);
            obj.Value1 = 20;
            var dbSet = context.Set<DomainObjectA>();
            dbSet.Add(obj);

            context.SaveChangesAsync();
        }

        using (var context = new TestDbContext(connection))
        {
            context.Database.EnsureCreated();
            var result = context.Set<DomainObjectA>().SingleOrDefault(e => e.Id == id);
            Assert.NotNull(result);
            Assert.Equal("7934B10B-9575-4966-A477-8DD1F2BCE140", result?.TypeId);
            Assert.Equal(20, result?.Value1);
        }        
    }

    [Fact]
    public void Test_002_AddAndQueryAuditableAggregateRoot()
    {
        var id = Guid.NewGuid();
        var principalId = Guid.NewGuid();

        AggregateTypeIdProvider.ReadAllTypes(new[] { typeof(AuditableDomainObjectA) });
        DomainEventTypeIdProvider.ReadAllTypes(new[] { typeof(AuditableDomainObjectA) });

        using var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        using (var context = new TestDbContext(connection))
        {
            context.Database.EnsureCreated();

            var obj = new AuditableDomainObjectA(id, principalId);
            obj.SetValue1(20, principalId);
            var dbSet = context.Set<AuditableDomainObjectA>();
            dbSet.Add(obj);

            context.SaveChangesAsync();
        }

        using (var context = new TestDbContext(connection))
        {
            context.Database.EnsureCreated();

            var result = context.Set<AuditableDomainObjectA>().SingleOrDefault(e => e.Id == id);
            Assert.NotNull(result);
            Assert.Equal("F48DBCE9-EC33-4917-B2D4-3FB46146C12A", result?.TypeId);
            Assert.Equal(principalId, result?.CreatedBy);
            Assert.NotNull(result?.CreatedAt);
            Assert.Equal(20, result?.Value1);
            Assert.Equal(principalId, result?.ModifiedBy);
            Assert.NotNull(result?.ModifiedAt);
        }
    }
}