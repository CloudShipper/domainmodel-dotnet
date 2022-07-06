using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test.Infrastructure;

internal class TestDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("testingdbcontext");
        optionsBuilder.ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Add Aggregate DomainObjectA
        var aggregate = modelBuilder.Aggregate<DomainObjectA, Guid>();

        aggregate
            .Property(x => x.Value1)
            .HasColumnName("value1");

        // Add AuditableAggregate AuditableDomainObjectA
        var auditable = modelBuilder.Aggregate<AuditableDomainObjectA, Guid, Guid>();

        auditable
            .Property(x => x.Value1)
            .HasColumnName("value1");

        base.OnModelCreating(modelBuilder);
    }
}
