using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();
app.UseCloudEvents();
app.UseEndpoints(endpoints =>
{
    endpoints.MapSubscribeHandler();
});

app.MapPost("/dsstatus", ([FromBody] string word) => Console.WriteLine($"Hello {word}!")).WithTopic("pubsub", "deathStarStatus");

app.Run();
