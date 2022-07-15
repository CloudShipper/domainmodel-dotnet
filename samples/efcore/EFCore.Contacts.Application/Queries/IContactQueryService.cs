using EFCore.Contacts.Application.Queries.Models;

namespace EFCore.Contacts.Application.Queries;

public interface IContactQueryService
{
    Task<ContactDto> GetByIdAsync(Guid id);
    Task<IEnumerable<ContactDto>> GetAllasync();
}
