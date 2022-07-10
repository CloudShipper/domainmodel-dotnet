using CloudShipper.DomainModel.Events;
using CloudShipper.DomainModel.MediatR;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceConfiguration
{
    public static IServiceCollection AddMediatRDispatcher(this IServiceCollection services)
    {
        return services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
    }
}
