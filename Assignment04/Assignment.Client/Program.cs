using Dapr.Client;

var appId = "assignment-server";

var client = new DaprClientBuilder().Build();

await client.InvokeMethodAsync(appId, "/");

var resp = await client.InvokeMethodAsync<string>(HttpMethod.Get, appId, "Hello1");
Console.WriteLine($"Hello1 Response: {resp}");

await client.InvokeMethodAsync(appId, "Hello2");

await client.InvokeMethodAsync(appId, "Hello3");