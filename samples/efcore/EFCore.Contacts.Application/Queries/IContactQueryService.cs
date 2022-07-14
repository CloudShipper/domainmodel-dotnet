using EFCore.Contacts.Domain;

namespace EFCore.Contacts.Application.Queries;

public interface IContactQueryService
{
    Task<Contact> GetByIdAsync(Guid id);
    Task<IEnumerable<Contact>> GetAllasync();
}
