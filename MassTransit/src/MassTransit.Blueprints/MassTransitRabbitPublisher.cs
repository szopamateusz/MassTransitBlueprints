using System;
using System.Globalization;
using System.Threading.Tasks;
using ApplicationBlueprints.Correlation;

namespace MassTransit.Blueprints
{
    public interface IMassTransitRabbitPublisher
    {
        Task Publish<TEvent>(TEvent @event) where TEvent : class;
        Task Publish<TEvent>(TEvent @event, Guid correlationId) where TEvent : class;
    }

    public class MassTransitRabbitPublisher : IMassTransitRabbitPublisher
    {
        private readonly INewIdGenerator _newIdGenerator;
        private readonly ICorrelationIdStore _correlationIdStore;
        private readonly IBus _bus;

        public MassTransitRabbitPublisher(INewIdGenerator newIdGenerator, ICorrelationIdStore correlationIdStore, IBus bus)
        {
            _newIdGenerator = newIdGenerator;
            _correlationIdStore = correlationIdStore;
            _bus = bus;
        }

        public async Task Publish<TEvent>(TEvent @event) where TEvent : class
        {
            var correlationId = _correlationIdStore.GetCorrelationId() ?? _newIdGenerator.NextGuid();
            var eventToPublish = Convert.ChangeType(@event, @event.GetType(), CultureInfo.InvariantCulture);

            await _bus.Publish(eventToPublish, x =>
            {
                x.CorrelationId = correlationId; 

            });
        }

        public async Task Publish<TEvent>(TEvent @event, Guid correlationId) where TEvent : class
        {
            var eventToPublish = Convert.ChangeType(@event, @event.GetType(), CultureInfo.InvariantCulture);

            await _bus.Publish(eventToPublish, x =>
            {
                x.CorrelationId = correlationId;

            });
        }
    }
}
