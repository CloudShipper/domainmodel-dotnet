using FluentValidation;

namespace EFCore.Contacts.Application.Commands.Contact.Validators
{
    public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
    {
        public CreateContactCommandValidator()
        {
            RuleFor(x => x.PrincipalId).NotNull().NotEmpty();
            RuleFor(x => x.FirstName).NotNull().NotEmpty();
            RuleFor(x => x.LastName).NotNull().NotEmpty();
            RuleFor(x => x.Description).NotNull().NotEmpty();
            RuleFor(x => x.Email).NotNull().NotEmpty();
        }
    }
}
