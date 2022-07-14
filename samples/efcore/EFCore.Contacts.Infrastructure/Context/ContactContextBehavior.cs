using MediatR;

namespace EFCore.Contacts.Infrastructure.Context;

public class ContactContextBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ContactContext _context;

    public ContactContextBehavior(ContactContext context)
    {
        _context = context;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        await _context.Database.EnsureCreatedAsync(cancellationToken);
        return await next();
    }
}
