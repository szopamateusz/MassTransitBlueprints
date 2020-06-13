using System.Collections.Generic;
using System.Linq;
using GreenPipes;

namespace MassTransit.Blueprints.SerilogCorrelatedLogs
{
    public class SerilogCorrelatedLoggerFilterSpecification<T> : IPipeSpecification<T> where T : class, ConsumeContext
    {
        public void Apply(IPipeBuilder<T> builder)
        {
            builder.AddFilter(new SerilogCorrelatedLoggerFilter<T>());
        }

        public IEnumerable<ValidationResult> Validate()
        {
            return Enumerable.Empty<ValidationResult>();
        }
    }
}
