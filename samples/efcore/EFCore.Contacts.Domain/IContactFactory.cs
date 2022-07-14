using CloudShipper.DomainModel.Aggregate;

namespace EFCore.Contacts.Domain;

public interface IContactFactory : IAuditableAggregateRootFactory<Contact, Guid, Guid>
{
    Contact Create(Guid id, Guid principalId, string firstname, string lastname, string description, string email);
}
