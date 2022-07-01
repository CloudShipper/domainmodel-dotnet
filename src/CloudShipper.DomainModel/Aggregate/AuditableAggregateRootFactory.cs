using System.Linq.Expressions;
using System.Reflection;

namespace CloudShipper.DomainModel.Aggregate;

public class AuditableAggregateRootFactory<TAggregateRoot, TId, TPrincipalId> 
    : IAuditableAggregateRootFactory<TAggregateRoot, TId, TPrincipalId>
    where TAggregateRoot : class, IAuditableAggregateRoot<TId, TPrincipalId>
{
    private static Func<TId, TPrincipalId, TAggregateRoot>? _creator = null;

    static AuditableAggregateRootFactory()
    {
        var constructor = typeof(TAggregateRoot).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                               null, new[] { typeof(TId), typeof(TPrincipalId) }, null);

        if (null == constructor)
            return;

        var pId = Expression.Parameter(typeof(TId));
        var pPrincipalId = Expression.Parameter(typeof(TPrincipalId));
        var createExpression = Expression.Lambda<Func<TId, TPrincipalId, TAggregateRoot>>(
            Expression.New(constructor, new Expression[] { pId, pPrincipalId }), pId, pPrincipalId);
        _creator = createExpression.Compile();
    }

    public virtual TAggregateRoot Create(TId id, TPrincipalId principalId)
    {
        if (null == _creator)
            throw new InvalidOperationException(
                $"Unable to create an instance of '{typeof(TAggregateRoot)}' using c'tor '{typeof(TAggregateRoot).Name}({typeof(TId).Name} {nameof(id)}, {typeof(TPrincipalId).Name} {nameof(principalId)})'");

        return _creator(id, principalId);
    }
}
