using CloudShipper.DomainModel.Repository;
using EFCore.Contacts.Domain;

namespace EFCore.Contacts.Application.Queries;

public class ContactQueryService : IContactQueryService
{
    private readonly IAuditableAggregateRootRepository<Contact, Guid, Guid> _repository;

    public ContactQueryService(IAuditableAggregateRootRepository<Contact, Guid, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Contact>> GetAllasync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Contact> GetByIdAsync(Guid id)
    {
        return await _repository.GetAsync(id);
    }
}
