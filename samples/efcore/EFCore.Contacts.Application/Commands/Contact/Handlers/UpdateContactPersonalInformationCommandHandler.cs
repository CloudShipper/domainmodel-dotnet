using CloudShipper.DomainModel.Repository;
using EFCore.Contacts.Application.Commands.Contact;
using EFCore.Contacts.Application.Commands.Contact.Responses;
using MediatR;

namespace EFCore.Contacts.Application.Commands.Contact.Handlers;

public class UpdateContactPersonalInformationCommandHandler : IRequestHandler<UpdateContactPersonalInformationCommand,
    UpdateContactPersonalInformationCommandResponse>
{
    private readonly IAuditableAggregateRootRepository<Domain.Contact, Guid, Guid> _repository;

    public UpdateContactPersonalInformationCommandHandler(IAuditableAggregateRootRepository<Domain.Contact, Guid, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<UpdateContactPersonalInformationCommandResponse> Handle(UpdateContactPersonalInformationCommand request,
        CancellationToken cancellationToken)
    {
        var contact = await _repository.GetAsync(request.Id);

        contact.UpdatePersonalInformation(request.PrincipalId,
                                          request.FirstName,
                                          request.LastName,
                                          request.Description,
                                          request.Email);

        await _repository.SaveAsync(contact);

        return new UpdateContactPersonalInformationCommandResponse(contact.Id);
    }
}
