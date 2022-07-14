using CloudShipper.DomainModel.Aggregate;

namespace EFCore.Contacts.Domain
{
    internal class ContactFactory : AuditableAggregateRootFactory<Contact, Guid, Guid>, IContactFactory
    {
        public Contact Create(Guid id, Guid principalId, string firstname,
                        string lastname,
                        string description,
                        string email)
        {
            return new Contact(id, principalId, firstname, lastname, description, email);
        }
    }
}
