var client = new Dapr.Client.DaprClientBuilder().Build();
await client.PublishEventAsync<string>("pubsub", "deathStarStatus", "World");
