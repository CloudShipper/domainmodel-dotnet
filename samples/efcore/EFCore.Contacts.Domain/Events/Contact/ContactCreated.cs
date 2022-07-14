using CloudShipper.DomainModel.Events;
using MediatR;

namespace EFCore.Contacts.Domain.Events.Contact;

[DomainEvent("C2B6E87C-8E07-4BDC-B4E9-8438C0F026AD")]
public class ContactCreated : AuditableDomainEvent<Domain.Contact, Guid, Guid>, INotification
{
    public ContactCreated(
        Guid aggregateId, 
        Guid principalid,
        string firstname,
        string lastname,
        string description,
        string email) : base(aggregateId,
                             principalid)
    {
        Firstname = firstname;
        Lastname = lastname;
        Description = description;
        Email = email;
    }

    public string Firstname { get; private set;  }
    public string Lastname { get; private set; }
    public string Description { get; private set; }
    public string Email { get; private set; }
}
