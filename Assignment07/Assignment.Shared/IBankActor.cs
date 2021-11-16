using Dapr.Actors;

namespace Assignment.Shared;
public interface IBankActor : IActor
{
    Task<AccountBalance> GetAccountBalance();

    Task Withdraw(WithdrawRequest withdraw);
}