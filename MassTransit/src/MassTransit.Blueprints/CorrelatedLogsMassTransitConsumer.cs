using System;
using System.Threading.Tasks;
using ApplicationBlueprints.Correlation;

namespace MassTransit.Blueprints
{
    public abstract class CorrelatedLogsMassTransitConsumer<TEvent> : IConsumer<TEvent> where TEvent : class
    {
        private readonly ICorrelationIdStore _correlationIdStore;

        protected CorrelatedLogsMassTransitConsumer(ICorrelationIdStore correlationIdStore)
        {
            _correlationIdStore = correlationIdStore;
        }

        public async Task Consume(ConsumeContext<TEvent> context)
        {
            _correlationIdStore.SetCorrelationId(context.CorrelationId);

            await DoConsume(context);
        }

        protected abstract Task DoConsume(ConsumeContext<TEvent> context);
    }
}
