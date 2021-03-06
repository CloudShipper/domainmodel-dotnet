using System.Linq.Expressions;
using System.Reflection;

namespace CloudShipper.DomainModel.Aggregate;

public class AggregateRootFactory<TAggregate, TId> : IAggregateRootFactory<TAggregate, TId>
    where TAggregate : class, IAggregateRoot<TId>
{
    private static Func<TId, TAggregate>? _creator = null;

    static AggregateRootFactory()
    {
        var constructor = typeof(TAggregate).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                               null, new[] { typeof(TId) }, null);

        if (null == constructor)
            return;

        var parameter = Expression.Parameter(typeof(TId));
        var createExpression = Expression.Lambda<Func<TId, TAggregate>>(
            Expression.New(constructor, new Expression[] { parameter }), parameter);
        _creator = createExpression.Compile();
    }
    

    public virtual TAggregate Create(TId id)
    {        
        if (null == _creator)
            throw new InvalidOperationException(
                $"Unable to create an instance of '{typeof(TAggregate)}' using c'tor '{typeof(TAggregate).Name}({typeof(TId).Name} {nameof(id)})'");

        return _creator(id);
    }
}
