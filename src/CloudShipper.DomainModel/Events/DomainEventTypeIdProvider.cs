using System.Reflection;

namespace CloudShipper.DomainModel.Events;

internal static class DomainEventTypeIdProvider
{
    private static int _initialized = 0;
    private static Dictionary<Type, string> _typeTotypeIds = new();
    private static Dictionary<string, Type> _typeIdToType = new();

    public static string Get(IDomainEvent @event)
    {
        if (null == @event)
            throw new ArgumentNullException(nameof(@event));

        if (!_typeTotypeIds.TryGetValue(@event.GetType(), out string? result))
            throw new KeyNotFoundException($"No type id found for {@event.GetType().FullName}");

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
        if (0 != Interlocked.CompareExchange(ref _initialized, 1, 0))
        {
            return;
        }

        foreach (var type in types)
        {
            var events = Assembly.GetAssembly(type)?.GetTypes()
                .Where(t => t.GetCustomAttribute<DomainEventAttribute>() != null)
                .ToList();
            if (null == events)
                continue;

            foreach (var e in events)
            {
                if (!e.IsAssignableTo(typeof(IDomainEvent)))
                    continue;

                var attr = e.GetCustomAttribute<DomainEventAttribute>();
                if (null == attr)
                    continue;

                _typeTotypeIds.Add(e, attr.EventTypeId);
                _typeIdToType.Add(attr.EventTypeId, e);
            }
        }
    }
}
