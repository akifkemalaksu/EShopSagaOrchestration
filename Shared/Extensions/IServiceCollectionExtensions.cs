using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Shared.Commands;
using Shared.Interfaces.Commands;
using Shared.Interfaces.Queries;
using Shared.Interfaces.Services;
using Shared.Queries;
using Shared.Services;

namespace Shared.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IBusService, BusService>();

            services.AddScoped(serviceProvider => new Lazy<IPublishEndpoint>(() => serviceProvider.GetRequiredService<IPublishEndpoint>()));
            services.AddScoped(serviceProvider => new Lazy<ISendEndpointProvider>(() => serviceProvider.GetRequiredService<ISendEndpointProvider>()));

            return services;
        }

        public static IServiceCollection ConfigureCQRSServices(this IServiceCollection services)
        {
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();

            services.Scan(selector =>
            {
                selector.FromEntryAssembly()
                .AddClasses(filter =>
                {
                    filter.AssignableTo(typeof(IQueryHandler<,>));
                })
                .AsImplementedInterfaces()
                .WithScopedLifetime();
            });

            services.Scan(selector =>
            {
                selector.FromEntryAssembly()
                .AddClasses(filter =>
                {
                    filter.AssignableTo(typeof(ICommandHandler<,>));
                })
                .AsImplementedInterfaces()
                .WithScopedLifetime();
            });

            return services;
        }
    }
}
