using Assignment.Shared;
using Dapr.Actors;
using Dapr.Actors.Client;

Console.WriteLine("Creating a Bank Actor");
var bank = ActorProxy.Create<IBankActor>(ActorId.CreateRandom(), "BankActor");
Parallel.ForEach(Enumerable.Range(1, 10), async i =>
{
    while (true)
    {
        var balance = await bank.GetAccountBalance();
        Console.WriteLine($"[Worker-{i}] Balance for account '{balance.AccountId}' is '{balance.Balance:c}'.");

        Console.WriteLine($"[Worker-{i}] Withdrawing '{1m:c}'...");
        try
        {
            await bank.Withdraw(new WithdrawRequest() { Amount = 1m });
        }
        catch (ActorMethodInvocationException ex)
        {
            Console.WriteLine("[Worker-{i}] Overdraft: " + ex.Message);
        }

        Task.Delay(1000).Wait();
    }
});

Console.ReadKey();