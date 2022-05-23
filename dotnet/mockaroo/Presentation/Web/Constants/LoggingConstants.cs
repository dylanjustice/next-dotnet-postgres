namespace Mockaroo.Presentation.Web.Constants
{
    public class LoggingConstants
    {
        public const string APP_NAME = "MockarooProxy";
        public const string OUTPUT_TEMPLATE = "{MachineName} {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{Application}] [{SourceContext}] {Message:lj}{NewLine}{Exception}{NewLine}";
        public const string CLOUDWATCH_LOG_GROUP = "dylanjustice-demo/mockaroo";
    }
}