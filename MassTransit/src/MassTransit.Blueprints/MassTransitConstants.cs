namespace MassTransit.Blueprints
{
    internal static class MassTransitConstants
    {
        internal const string CorrelationIdHeaderName = "CorrelationId";
        internal const string MessageIdHeaderName = "MessageId";
        internal const string SerilogCorrelationIdHeaderName = "AppCorrelationId";
        internal const string SerilogFilterScope = "correlatedLogsFilter";
    }
}
