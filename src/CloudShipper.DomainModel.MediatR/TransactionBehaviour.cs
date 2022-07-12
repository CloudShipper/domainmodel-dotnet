using CloudShipper.DomainModel.Infrastructure;
using MediatR;

namespace CloudShipper.DomainModel.MediatR;

public class TransactionBehaviour<TContext, TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TContext : class
{
    private readonly IUnitOfWork<TContext> _unitOfWork;

    public TransactionBehaviour(IUnitOfWork<TContext> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var context = _unitOfWork as ITransactionable;

        if (null == context)
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
