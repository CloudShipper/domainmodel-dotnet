using CloudShipper.DomainModel.Aggregate;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceConfiguration
{
    public static IServiceCollection AddDomain(this IServiceCollection services, IEnumerable<Type> domainTypes)
    {
        // 1. initalize AggregateTypeIdProvider
        AggregateTypeIdProvider.ReadAllTypes(domainTypes);


        return services;
    }
}
