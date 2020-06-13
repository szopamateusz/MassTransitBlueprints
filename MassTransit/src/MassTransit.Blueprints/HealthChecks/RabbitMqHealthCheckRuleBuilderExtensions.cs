using System;
using System.Net;
using MassTransit.Blueprints.Settings;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace MassTransit.Blueprints.HealthChecks
{
    public static class RabbitMqHealthCheckRuleBuilderExtensions
    {
        public static IServiceCollection AddRabbitMqHealthChecks<TConnectionSettings>(this IServiceCollection services,
            IServiceProvider serviceProvider,
            string healthChecksSuffix = null)
            where TConnectionSettings : IRabbitMqConnectionSettings
        {
            var settings = serviceProvider.GetService<TConnectionSettings>();

            services.AddRabbitMqHealthChecks(settings, healthChecksSuffix);

            return services;
        }

        private static IServiceCollection AddRabbitMqHealthChecks(this IServiceCollection services,
            IRabbitMqConnectionSettings connectionSettings, string nameSuffix = null)
        {
            if(string.IsNullOrEmpty(connectionSettings.Url.ToString()))
                throw new ArgumentNullException(nameof(connectionSettings));

            var name = CreateName("rabbitMq", nameSuffix);
            var encodedLogin = WebUtility.UrlEncode(connectionSettings.Login);
            var encodedPassword = WebUtility.UrlEncode(connectionSettings.Password);
            var port = connectionSettings.Url.IsDefaultPort
                ? RabbitMqConnectionSettingsBase.DefaultPort
                : connectionSettings.Url.Port;

            var connectionString = $"amqp://{encodedLogin}:{encodedPassword}@{connectionSettings.Url.Host}:{port}";

            services.AddHealthChecks()
                .AddRabbitMQ(connectionString, new SslOption(), name);

            return services;
        }
        private static string CreateName(string namePrefix, string nameSuffix)
        {
            return nameSuffix != null ? $"{namePrefix} - {nameSuffix}" : null;
        }
    }
}
