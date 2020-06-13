using System;


namespace MassTransit.Blueprints.Settings
{
    public class RabbitMqConnectionSettingsBase : IRabbitMqConnectionSettings
    {
        public const int DefaultPort = 5672;

        public Uri Url { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
