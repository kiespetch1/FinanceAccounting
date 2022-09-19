namespace FinanceAccounting.Exceptions;

public class WrongCredentialsException : Exception
{
    public WrongCredentialsException(string message)
        : base(message) { }
}