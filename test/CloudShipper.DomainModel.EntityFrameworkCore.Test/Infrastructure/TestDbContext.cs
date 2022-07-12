using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test.Infrastructure;

internal class TestDbContext : DbContext
{
    private readonly SqliteConnection? _connection;
    public TestDbContext(SqliteConnection connection)
    {
        _connection = connection;
    }

    public TestDbContext()
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (null == _connection)
            optionsBuilder.UseSqlite("DataSource=:memory:");
        else
            optionsBuilder.UseSqlite(_connection);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Add Aggregate DomainObjectA
        var aggregate = modelBuilder.Aggregate<DomainObjectA, Guid>();

        aggregate.ToTable("domainObjectA");

        aggregate
            .Property(x => x.Value1)
            .HasColumnName("value1");

        // Add AuditableAggregate AuditableDomainObjectA
        var auditable = modelBuilder.Aggregate<AuditableDomainObjectA, Guid, Guid>();

        auditable.ToTable("AuditableDomainObjectA");

        auditable
            .Property(x => x.Value1)
            .HasColumnName("value1");

        base.OnModelCreating(modelBuilder);
    }
}
