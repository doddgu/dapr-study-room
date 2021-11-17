using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenTelemetryTracing((tracerProviderBuilder) =>
    tracerProviderBuilder
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("testobservability"))
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddZipkinExporter(zipkinOptions =>
        {
            zipkinOptions.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
        }
    )
);
var app = builder.Build();

app.Map("/Amazing", async (HttpContext context) =>
{
    if (context.Request.Headers.TryGetValue("traceparent", out var traceparent))
    {
        Console.WriteLine($"TraceParent: {traceparent}");
    }
    if (context.Request.Headers.TryGetValue("tracestate", out var tracestate))
    {
        Console.WriteLine($"TraceState: {tracestate}");
    }

    System.Diagnostics.Activity.Current?.SetParentId(traceparent.ToString());
    _ = await new HttpClient().GetStringAsync("https://www.baidu.com");
    Console.WriteLine($"Invoke succeed: traceID:{traceparent}");
});

app.Run();
