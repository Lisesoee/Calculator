using System.Diagnostics;
using System.Reflection;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
//using Serilog.Enrichers.Span;

namespace Calculator.Services
{
    public static class MonitorService
    {
        public static readonly string ServiceName = Assembly.GetCallingAssembly().GetName().Name ?? "Unknown";
        public static TracerProvider TracerProvider;
        public static ActivitySource ActivitySource = new ActivitySource(ServiceName);
        //public static ILogger Log => Serilog.Log.Logger;
        public static Serilog.ILogger Log => Serilog.Log.Logger;

        static MonitorService()
        {
            // OpenTelemetry
            TracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddConsoleExporter()
                .AddZipkinExporter(config =>
                {
                    config.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
                })
                .AddSource(ActivitySource.Name)
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(ServiceName))
                .Build();

            // Serilog
            Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .WriteTo.Seq("http://seq:5341")
                //.Enrich.WithSpan() // enrich logs with spans from traced activities
                .CreateLogger();
        }
    }
}
