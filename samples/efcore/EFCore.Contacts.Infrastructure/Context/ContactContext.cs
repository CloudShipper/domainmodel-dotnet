using Microsoft.EntityFrameworkCore;

namespace EFCore.Contacts.Infrastructure.Context;

public class ContactContext : DbContext
{
    public ContactContext(DbContextOptions<ContactContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var contact = modelBuilder.Aggregate<Domain.Contact, Guid, Guid>();
        contact.ToTable("tbl_contacts");
        contact.Property(c => c.FirstName).HasColumnName("firstname");
        contact.Property(c => c.LastName).HasColumnName("lastname");
        contact.Property(c => c.Description).HasColumnName("description");
        contact.Property(c => c.Email).HasColumnName("email");

        base.OnModelCreating(modelBuilder);
    }
}
