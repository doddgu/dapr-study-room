using Assignment.Shared;
using Dapr.Actors.Runtime;
using System;

namespace Assignment.Server;
public class BankActor : Actor, IBankActor, IRemindable // IRemindable is not required
{
    private readonly BankService bank;

    public BankActor(ActorHost host, BankService bank)
        : base(host)
    {
        // BankService is provided by dependency injection.
        // See Program.cs
        this.bank = bank;
    }

    public async Task<AccountBalance> GetAccountBalance()
    {
        var starting = new AccountBalance()
        {
            AccountId = this.Id.GetId(),
            Balance = 10m, // Start new accounts with 100, we're pretty generous.
        };

        var balance = await StateManager.GetOrAddStateAsync("balance", starting);
        return balance;
    }

    public async Task Withdraw(WithdrawRequest withdraw)
    {
        var starting = new AccountBalance()
        {
            AccountId = this.Id.GetId(),
            Balance = 10m, // Start new accounts with 100, we're pretty generous.
        };

        var balance = await StateManager.GetOrAddStateAsync("balance", starting)!;

        if (balance.Balance <= 0)
        {
            // Simulated reminder deposit
            if (Random.Shared.Next(100) > 90)
            {
                await RegisterReminderAsync("Deposit", null, TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(-1));
            }
        }

        // Throws Overdraft exception if the account doesn't have enough money.
        var updated = this.bank.Withdraw(balance.Balance, withdraw.Amount);

        balance.Balance = updated;
        await StateManager.SetStateAsync("balance", balance);
    }

    public async Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
    {
        if (reminderName == "Deposit")
        {
            var balance = await StateManager.GetStateAsync<AccountBalance>("balance")!;

            if (balance.Balance <= 0)
            {
                balance.Balance += 60; // 50(Overdraft Threshold) + 10 = 60
                Console.WriteLine("Deposit: 10");
            }
            else
            {
                Console.WriteLine("Deposit: ignore");
            }
        }
    }
}
