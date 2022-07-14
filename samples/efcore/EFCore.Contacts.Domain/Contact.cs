using CloudShipper.DomainModel.Aggregate;
using EFCore.Contacts.Domain.Events.Contact;

namespace EFCore.Contacts.Domain
{
    [Aggregate("273D836C-EE64-4E72-ADA2-9B733D009D9A")]
    public class Contact : AuditableAggregateRoot<Guid, Guid>
    {
        private Contact()
            : base(Guid.Empty, Guid.Empty)
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Description = string.Empty;
            Email = string.Empty;
        }

        internal Contact(Guid id,
                        Guid principalId,
                        string firstname,
                        string lastname,
                        string description,
                        string email) : base(id, principalId)
        {
            FirstName = firstname;
            LastName = lastname;
            Description = description;
            Email = email;

            var @event = new ContactCreated(id, principalId, firstname, lastname, description, email);
            CreatedAt = @event.Timestamp;

            AddEvent(@event);
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Description { get; private set; }
        public string Email { get; private set; }

        public void UpdatePersonalInformation(Guid raisedBy, string firstname, string lastname, string description, string email)
        {
            FirstName = firstname;
            LastName = lastname;
            Description = description;
            Email = email;

            var @event = new ContactPersonalInformationUpdated(Id, raisedBy, firstname, lastname, description, email);
            ModifiedBy = raisedBy;
            ModifiedAt = @event.Timestamp;

            AddEvent(@event);
        }
    }
}