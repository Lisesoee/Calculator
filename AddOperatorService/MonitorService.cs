using System.Diagnostics;
using System.Reflection;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
//using Serilog.Enrichers.Span;

namespace AddOperatorService
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
                .AddZipkinExporter()
                .AddSource(ActivitySource.Name)
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(ServiceName))
                .Build();

            // Serilog
            Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .WriteTo.Seq("http://localhost:5341")
                //.Enrich.WithSpan() // enrich logs with spans from traced activities
                .CreateLogger();
        }
    }
}
