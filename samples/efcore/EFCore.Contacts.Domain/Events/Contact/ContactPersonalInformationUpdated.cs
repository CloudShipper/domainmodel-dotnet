using CloudShipper.DomainModel.Events;
using MediatR;

namespace EFCore.Contacts.Domain.Events.Contact;

[DomainEvent("BAB785DF-1E88-4084-AD89-9D3412FAF7A1")]
public class ContactPersonalInformationUpdated : AuditableDomainEvent<Domain.Contact, Guid, Guid>, INotification
{
    public ContactPersonalInformationUpdated(
        Guid aggregateId, 
        Guid principalid,
        string firstname,
        string lastname,
        string description,
        string email) : base(aggregateId, principalid)
    {
        Firstname = firstname;
        Lastname = lastname;
        Description = description;
        Email = email;
    }

    public string Firstname { get; private set; }
    public string Lastname { get; private set; }
    public string Description { get; private set; }
    public string Email { get; private set; }
}
