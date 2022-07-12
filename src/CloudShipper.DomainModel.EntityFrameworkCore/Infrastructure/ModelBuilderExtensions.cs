using CloudShipper.DomainModel.Aggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore;

public static class ModelBuilderExtensions
{
    public static EntityTypeBuilder<TAggregate> Aggregate<TAggregate, TAggregateId>(this ModelBuilder modelBuilder)
        where TAggregate : class, IAggregateRoot<TAggregateId>
    {
        var entity = modelBuilder.Entity<TAggregate>();

        entity
            .HasKey(x => x.Id)
            .HasName("id");

        entity
            .Ignore(x => x.DomainEvents);

        entity
            .Ignore(x => x.TypeId);

        return entity;
    }

    public static EntityTypeBuilder<TAggregate> Aggregate<TAggregate, TAggregateId, TPrincipalId>(this ModelBuilder modelBuilder)
        where TAggregate : class, IAuditableAggregateRoot<TAggregateId, TPrincipalId>
    {
        var entity = modelBuilder.Entity<TAggregate>();

        entity
            .HasKey(x => x.Id)
            .HasName("id");

        entity
            .Ignore(x => x.DomainEvents);

        entity
            .Ignore(x => x.TypeId);

        entity
            .Property(x => x.CreatedBy)
            .HasColumnName("createdBy")
            .IsRequired();

        entity
            .Property(x => x.CreatedAt)
            .HasColumnName("createdAt")
            .IsRequired();

        entity
            .Property(x => x.ModifiedBy)
            .HasColumnName("modifiedBy");

        entity
            .Property(x => x.ModifiedAt)
            .HasColumnName("modifiedAt");

        entity
            .Property(x => x.DeletedBy)
            .HasColumnName("deletedBy");

        entity
            .Property(x => x.DeletedAt)
            .HasColumnName("deletedAt");

        return entity;
    }
}
