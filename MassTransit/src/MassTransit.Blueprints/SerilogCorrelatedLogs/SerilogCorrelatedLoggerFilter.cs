using System.Threading.Tasks;
using GreenPipes;
using Serilog.Context;

namespace MassTransit.Blueprints.SerilogCorrelatedLogs
{
    public class SerilogCorrelatedLoggerFilter<T> : IFilter<T> where T : class, ConsumeContext
    {
        public async Task Send(T context, IPipe<T> next)
        {
            using (LogContext.PushProperty(MassTransitConstants.SerilogCorrelationIdHeaderName, context.CorrelationId))
            {
                await next.Send(context);
            }
        }

        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope(MassTransitConstants.SerilogFilterScope);
        }
    }
}
