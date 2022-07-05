using CloudShipper.DomainModel.Aggregate;
using CloudShipper.DomainModel.Events;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceConfiguration
{
    public static IServiceCollection AddDomain(this IServiceCollection services, IEnumerable<Type> domainTypes)
    {
        // 1. initalize AggregateTypeIdProvider
        AggregateTypeIdProvider.ReadAllTypes(domainTypes);

        // 2. initialize DomainEventTypeIdProvider
        DomainEventTypeIdProvider.ReadAllTypes(domainTypes);

        // 3. add generic aggregate factories
        services
            .AddScoped(typeof(IAggregateRootFactory<,>), typeof(AggregateRootFactory<,>))
            .AddScoped(typeof(IAuditableAggregateRootFactory<,,>), typeof(AuditableAggregateRootFactory<,,>));

        // 4. Scan for all factories
        services.Scan(scan => scan
            .FromAssembliesOf(domainTypes)
                .AddClasses(classes => classes.AssignableTo(typeof(IAggregateRootFactory<,>)))
                    .AsSelfWithInterfaces()
                    .WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IAuditableAggregateRootFactory<,,>)))
                    .AsSelfWithInterfaces()
                    .WithScopedLifetime()
            );
        return services;
    }
}
