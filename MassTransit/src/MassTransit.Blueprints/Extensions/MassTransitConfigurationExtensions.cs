using System;
using ApplicationBlueprints.Infrastructure.Configuration;
using GreenPipes;
using MassTransit.Blueprints.HealthChecks;
using MassTransit.AspNetCoreIntegration;
using MassTransit.Blueprints.SerilogCorrelatedLogs;
using MassTransit.Blueprints.Settings;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.Blueprints.Extensions
{
    public static class MassTransitConfigurationExtensions
    {
        public static IRabbitMqHost ConfigureHost<TConnectionSettings>(
            this IRabbitMqBusFactoryConfigurator configurator,
            IServiceProvider serviceProvider,
            Action<IRabbitMqHostConfigurator> additionalConfiguration = null)
            where TConnectionSettings : IRabbitMqConnectionSettings
        {
            var connectionSettings = serviceProvider.GetService<TConnectionSettings>();
            var host = configurator.Host(connectionSettings.Url, hostConfigurator =>
            {
                hostConfigurator.Username(connectionSettings.Login);
                hostConfigurator.Password(connectionSettings.Password);

                additionalConfiguration?.Invoke(hostConfigurator);
            });

            return host;
        }

        public static void ConfigureReceiveEndpoint<TConsumer>(
            this IRabbitMqBusFactoryConfigurator configurator,
            IServiceProvider serviceProvider,
            Action<IRabbitMqReceiveEndpointConfigurator> additionalConfiguration = null)
            where TConsumer : class, IConsumer
        {
            var consumerType = typeof(TConsumer);
            var queueName = $"{consumerType.Namespace}:{consumerType.Name}";

            configurator.ReceiveEndpoint(queueName, endpointConfigurator =>
            {
                endpointConfigurator.ConfigureConsumer<TConsumer>(serviceProvider);

                additionalConfiguration?.Invoke(endpointConfigurator);
            });
        }

        public static void UseCorrelatedLogger<T>(this IPipeConfigurator<T> configurator)
            where T : class, ConsumeContext
        {
            configurator.AddPipeSpecification(new SerilogCorrelatedLoggerFilterSpecification<T>());
        }

        public static IServiceCollection AddBasicMassTransit(
            this IServiceCollection services,
            IConfiguration configuration,
            string configurationSection,
            string healthChecksSuffix = null)
        {
            services.AddBasicRabbitMqSettings(configuration, configurationSection);

            services.AddMassTransit(provider =>
            {
                var bus = Bus.Factory.CreateUsingRabbitMq(busFactoryConfigurator =>
                {
                    busFactoryConfigurator.ConfigureHost<RabbitMqConnectionSettings>(provider);
                });

                services.AddRabbitMqHealthChecks<RabbitMqConnectionSettings>(provider, healthChecksSuffix);

                return bus;
            });

            return services;
        }

        public static IServiceCollection AddBasicRabbitMqSettings(
            this IServiceCollection services,
            IConfiguration configuration,
            string configurationSection)
        {
            return services.ConfigureSettings<RabbitMqConnectionSettings>(configuration, configurationSection);
        }
    }
}
