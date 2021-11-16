var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/myevent", ([Microsoft.AspNetCore.Mvc.FromBody] string word) => Console.WriteLine($"Hello {word}!"));

app.Run();
