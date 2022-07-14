using EFCore.Contacts.Application.Commands.Contact.Responses;
using MediatR;

namespace EFCore.Contacts.Application.Commands.Contact;

public class CreateContactCommand : IRequest<CreateContactCommandResponse>
{
    public Guid PrincipalId { get; set; } = Guid.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
