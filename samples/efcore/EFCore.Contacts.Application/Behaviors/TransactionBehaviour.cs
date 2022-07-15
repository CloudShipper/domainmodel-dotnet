using CloudShipper.DomainModel.Infrastructure;
using EFCore.Contacts.Infrastructure.Context;
using MediatR;

namespace EFCore.Contacts.Application.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IUnitOfWork<ContactContext> _unitOfWork;

    public TransactionBehavior(IUnitOfWork<ContactContext> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (_unitOfWork is not ITransactionProvider context)
        {
            return await next();
        }

        if (context.HasActiveTransaction())
        {
            return await next();
        }

        var response = default(TResponse);

        await context.NewResilientTransaction().ExecuteAsync(async () =>
        {
            response = await next();
        });

#pragma warning disable CS8603 // Possible null reference return.
        return response;
#pragma warning restore CS8603 // Possible null reference return.
    }
}
