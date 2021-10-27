var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Map("/hello", () => Console.WriteLine("Hello World!"));

app.Run();
