using CloudShipper.DomainModel.Aggregate;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceConfiguration
{
    public static IServiceCollection AddDomain(this IServiceCollection services, IEnumerable<Type> domainTypes)
    {
        // 1. initalize AggregateTypeIdProvider
        AggregateTypeIdProvider.ReadAllTypes(domainTypes);

        // 2. 

        // 2. Scan for all factories
        services.Scan(scan => scan
            .FromAssembliesOf(domainTypes)
                .AddClasses(classes => classes.AssignableTo(typeof(IAggregateRootFactory<,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IAuditableAggregateRootFactory<,,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
            );
        return services;
    }
}
