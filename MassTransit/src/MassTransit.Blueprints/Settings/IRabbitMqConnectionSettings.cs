using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Blueprints.Settings
{
    public interface IRabbitMqConnectionSettings
    {
         Uri? Url { get; }
         string Login { get; }
         string Password { get; }
    }
}
