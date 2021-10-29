var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/", () => Console.WriteLine("Hello!"));

app.MapGet("/Hello1", () =>
{
    Console.WriteLine("Hello World1!");
    return $"\"Hello World1!\"";
});

app.MapPost("/Hello2", () => Console.WriteLine("Hello World2!"));

app.Map("/Hello3", () => Console.WriteLine("Hello World3!"));

app.Run();
