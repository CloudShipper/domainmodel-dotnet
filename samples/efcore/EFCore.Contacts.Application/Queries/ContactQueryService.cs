using AutoMapper;
using CloudShipper.DomainModel.Repository;
using EFCore.Contacts.Application.Queries.Models;
using EFCore.Contacts.Domain;

namespace EFCore.Contacts.Application.Queries;

public class ContactQueryService : IContactQueryService
{
    private readonly IAuditableAggregateRootRepository<Contact, Guid, Guid> _repository;
    private readonly IMapper _mapper;

    public ContactQueryService(IAuditableAggregateRootRepository<Contact, Guid, Guid> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ContactDto>> GetAllasync()
    {
        var result = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ContactDto>>(result);
    }

    public async Task<ContactDto> GetByIdAsync(Guid id)
    {
        var result =  await _repository.GetAsync(id);
        return _mapper.Map<ContactDto>(result);
    }
}
