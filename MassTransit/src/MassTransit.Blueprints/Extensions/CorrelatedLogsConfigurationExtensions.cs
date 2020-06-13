using ApplicationBlueprints.Correlation;
using MassTransit.NewIdProviders;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.Blueprints.Extensions
{
    public static class CorrelatedLogsConfigurationExtensions
    {
        public static IServiceCollection AddMassTransitCorrelatedLogs(this IServiceCollection services)
        {
            services.AddCorrelationIdStore();
            services.AddScoped<ITickProvider, DateTimeTickProvider>();
            services.AddScoped<IWorkerIdProvider, BestPossibleWorkerIdProvider>();
            services.AddScoped<INewIdGenerator, NewIdGenerator>();
            services.AddScoped<IMassTransitRabbitPublisher, MassTransitRabbitPublisher>();

            return services;
        }
    }
}
