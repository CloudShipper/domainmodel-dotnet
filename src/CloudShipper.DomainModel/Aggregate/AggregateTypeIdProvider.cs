using CloudShipper.DomainModel.Entity;
using System.Reflection;

namespace CloudShipper.DomainModel.Aggregate;

internal static class AggregateTypeIdProvider
{
    private static int _initialized = 0;
    private static Dictionary<Type, string> _typeTotypeIds = new();
    private static Dictionary<string, Type> _typeIdToType = new();

    public static string Get(IAggregate aggregate)
    {
        if (null == aggregate)
            throw new ArgumentNullException(nameof(aggregate));

        if (!_typeTotypeIds.TryGetValue(aggregate.GetType(), out string? result))
            throw new KeyNotFoundException($"No type id found for {aggregate.GetType().FullName}");

        return result;
    }

    public static string Get(Type type)
    {
        if (!_typeTotypeIds.TryGetValue(type, out string? result))
            throw new KeyNotFoundException($"No type id found for type {type.FullName}");

        return result;
    }

    public static Type Get(string typeId)
    {
        if (!_typeIdToType.TryGetValue(typeId, out Type? result))
            throw new KeyNotFoundException($"No tpye found for type id {typeId}");

        return result;
    }

    public static void ReadAllTypes(IEnumerable<Type> types) 
    {
        // atomic barrier, provider can only be loaded once !!
        if (0 != Interlocked.CompareExchange(ref _initialized, 1 ,0))
        {
            return;
        }

        foreach (var type in types)
        {
            var aggregates = Assembly.GetAssembly(type)?.GetTypes()
                .Where(t => t.GetCustomAttribute<AggregateAttribute>() != null)
                .ToList();
            if (null == aggregates)
                continue;

            foreach (var a in aggregates)
            {
                if (!a.IsAssignableTo(typeof(IAggregate)))
                    continue;

                var attr = a.GetCustomAttribute<AggregateAttribute>();
                if (null == attr)
                    continue;

                _typeTotypeIds.Add(a, attr.AggregateTypeId);
                _typeIdToType.Add(attr.AggregateTypeId, a);
            }
        }
    }
}
