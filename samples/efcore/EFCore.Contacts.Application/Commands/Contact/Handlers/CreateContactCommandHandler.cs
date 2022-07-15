using CloudShipper.DomainModel.Repository;
using EFCore.Contacts.Application.Commands.Contact.Responses;
using EFCore.Contacts.Domain;
using MediatR;

namespace EFCore.Contacts.Application.Commands.Contact.Handlers;

public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, CreateContactCommandResponse>
{
    private readonly IContactFactory _contactFactory;
    private readonly IAuditableAggregateRootRepository<Domain.Contact, Guid, Guid> _repository;

    public CreateContactCommandHandler(IContactFactory contactFactory, IAuditableAggregateRootRepository<Domain.Contact, Guid, Guid> repository)
    {
        _contactFactory = contactFactory;
        _repository = repository;
    }


    public async Task<CreateContactCommandResponse> Handle(CreateContactCommand request, CancellationToken cancellationToken)
    {
        var contact = _contactFactory.Create(Guid.NewGuid(),
                                             request.PrincipalId,
                                             request.FirstName,
                                             request.LastName,
                                             request.Description,
                                             request.Email);

        await _repository.AddAsync(contact);

        return new CreateContactCommandResponse(contact.Id);
    }
}
