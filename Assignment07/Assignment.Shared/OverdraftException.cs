namespace Assignment.Shared;
public class OverdraftException : Exception
{
    public OverdraftException(decimal balance, decimal amount)
        : base($"Your current balance is {balance:c} - that's not enough to withdraw {amount:c}.")
    {
    }
}