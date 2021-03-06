using CloudShipper.DomainModel.EntityFrameworkCore;
using CloudShipper.DomainModel.EntityFrameworkCore.Infrastructure;
using CloudShipper.DomainModel.Infrastructure;
using CloudShipper.DomainModel.Repository;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceConfiguration
{
    public static IServiceCollection AddRepositories<TDbContext>(this IServiceCollection services, Action<IDbContextAggregateBinder<TDbContext>> binder)
        where TDbContext : DbContext
    {
        // Add UnitOfWork
        services
            .AddScoped<IUnitOfWork<TDbContext>, UnitOfWork<TDbContext>>();            
        
        var contextBinder = new DbContextAggregateBinder<TDbContext>();
        binder(contextBinder);

        foreach (var b in contextBinder.AggregateRootBindings)
        {
            var repoType = typeof(IAggregateRootRepository<,>).MakeGenericType(b.AggregateType, b.AggregateIdType);
            var repoImplType = typeof(AggregateRootRepository<,,>).MakeGenericType(b.DbContextType, b.AggregateType, b.AggregateIdType);

            services.AddScoped(repoType, repoImplType);
        }

        foreach (var b in contextBinder.AuditableAggregateRootBindings)
        {
            var repoType = typeof(IAuditableAggregateRootRepository<,,>).MakeGenericType(b.AggregateType, b.AggregateIdType, b.PrincipalIdType);
            var repoImplType = typeof(AuditableAggregateRootRepository<,,,>).MakeGenericType(b.DbContextType, b.AggregateType, b.AggregateIdType, b.PrincipalIdType);

            services.AddScoped(repoType, repoImplType);
        }

        return services;
    }

    public static IServiceCollection AddEFCoreDefaultRepositories(this IServiceCollection services, params Type[] assemblyTypes)
    {
        return services.Scan(scan => scan
            .FromAssembliesOf(assemblyTypes)
                .AddClasses(classes => classes.AssignableTo(typeof(IAggregateRootRepository<,>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime()
            );
    }
}