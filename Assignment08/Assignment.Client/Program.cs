using Dapr.Client;

var client = new DaprClientBuilder().Build();
await client.InvokeBindingAsync("myevent", "create", "World");

Console.WriteLine("Binding sent.");
