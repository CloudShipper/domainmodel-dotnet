using System.Reflection;

namespace CloudShipper.DomainModel.Aggregate;

internal static class AggregateTypeIdProvider
{
    private static int _initialized = 0;
    private static Dictionary<Type, string> _typeTotypeIds = new();
    private static Dictionary<string, Type> _typeIdToType = new();

    public static string Get(IAggregate aggregate)
    {
        return _typeTotypeIds[aggregate.GetType()];
    }

    public static string Get(Type type)
    {
        return _typeTotypeIds[type];
    }

    public static Type Get(string typeId)
    {
        return _typeIdToType[typeId];
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
            var aggregates = System.Reflection.Assembly.GetAssembly(type)?.GetTypes().Where(t => t.GetCustomAttribute<AggregateAttribute>() != null).ToList();
            if (null == aggregates)
                continue;

            foreach (var a in aggregates)
            {
                var attr = a.GetCustomAttribute<AggregateAttribute>();
                if (null == attr)
                    continue;

                _typeTotypeIds.Add(a, attr.AggregateTypeId);
                _typeIdToType.Add(attr.AggregateTypeId, a);
            }
        }
    }
}
