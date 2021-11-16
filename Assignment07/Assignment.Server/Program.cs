using Assignment.Server;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<BankService>();
builder.Services.AddActors(options =>
{
    options.Actors.RegisterActor<BankActor>();
});

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapActorsHandlers();
});

app.Run();