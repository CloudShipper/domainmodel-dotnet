using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain;
using Microsoft.EntityFrameworkCore;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test.Infrastructure;

internal class AnotherTestDbContext : DbContext
{
    public AnotherTestDbContext(DbContextOptions<AnotherTestDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("anotherTestDb");
        // Add Aggregate DomainObjectA
        var aggregate = modelBuilder.Aggregate<DomainObjectA, Guid>();

        aggregate.ToTable("domainObjectA");

        aggregate
            .Property(x => x.Value1)
            .HasColumnName("value1");
        base.OnModelCreating(modelBuilder);
    }
}
