using CloudShipper.DomainModel.EntityFrameworkCore.Test.Domain;
using Microsoft.EntityFrameworkCore;

namespace CloudShipper.DomainModel.EntityFrameworkCore.Test.Infrastructure;

internal class YetAnotherTestDbContext : DbContext
{
    public YetAnotherTestDbContext(DbContextOptions<YetAnotherTestDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("yetAnotherTestDb");

        // Add Aggregate DomainObjectA
        var aggregate = modelBuilder.Aggregate<DomainObjectB, Guid>();

        aggregate.ToTable("domainObjectB");

        aggregate
            .Property(x => x.Value1)
            .HasColumnName("value1");
        base.OnModelCreating(modelBuilder);
    }
}
