using Dapr.Client;

var storeName = "statestore";
var key = "myFirstKey";
var value = "myFirstValue";

var client = new DaprClientBuilder().Build();
var xx = await client.TryDeleteStateAsync(storeName, key, "2");
await client.SaveStateAsync(storeName, key, value);
Console.WriteLine("State has been stored");

var data = await client.GetStateAsync<string>(storeName, key);
Console.WriteLine($"Got value: {data}");

await client.DeleteStateAsync(storeName, key);
data = await client.GetStateAsync<string>(storeName, key);
Console.WriteLine($"Got value: {data}");

var lst = new List<StateTransactionRequest>()
{
    new StateTransactionRequest("test1", System.Text.Encoding.UTF8.GetBytes("value1"), StateOperationType.Upsert),
    new StateTransactionRequest("test2", System.Text.Encoding.UTF8.GetBytes("value2"), StateOperationType.Upsert),
};
await client.ExecuteStateTransactionAsync(storeName, lst);

var datas = await client.GetBulkStateAsync(storeName, lst.Select(r => r.Key).ToList(), 0);
Console.WriteLine($"Got items: {string.Join(",", datas.Select(d => $"{d.Key}={d.Value}"))}");

//Strong consistency
await client.SaveStateAsync(storeName, key, value, new StateOptions() { Consistency = ConsistencyMode.Strong });

var etagData = await client.GetStateAndETagAsync<string>(storeName, key, ConsistencyMode.Strong);
Console.WriteLine($"ETag:{etagData.etag}");

await client.DeleteStateAsync(storeName, key, new StateOptions() { Consistency = ConsistencyMode.Strong });

//First-write-wins
await client.SaveStateAsync(storeName, key, value, new StateOptions() { Concurrency = ConcurrencyMode.FirstWrite });
var firstWriteWinData = await client.GetStateAndETagAsync<string>(storeName, key);
var etag = firstWriteWinData.etag;

await client.TrySaveStateAsync(storeName, key, DateTime.Now.Ticks.ToString(), etag, new StateOptions() { Concurrency = ConcurrencyMode.FirstWrite });
var firstWriteWinDeleteSucceeded = await client.TryDeleteStateAsync(storeName, key, etag);
Console.WriteLine($"First write wins delete:{firstWriteWinDeleteSucceeded}");

firstWriteWinData = await client.GetStateAndETagAsync<string>(storeName, key);
firstWriteWinDeleteSucceeded = await client.TryDeleteStateAsync(storeName, key, firstWriteWinData.etag);
Console.WriteLine($"First write wins delete:{firstWriteWinDeleteSucceeded}");

//TTL
await client.SaveStateAsync(storeName, key, value, metadata: new Dictionary<string, string>() { { "ttlInSeconds", "3" } });
var ttlData = await client.GetStateAsync<string>(storeName, key);
Console.WriteLine($"TTL Data:{ttlData}");

Thread.Sleep(5000);
ttlData = await client.GetStateAsync<string>(storeName, key);
Console.WriteLine($"TTL Data:{ttlData}");

Console.ReadKey();
