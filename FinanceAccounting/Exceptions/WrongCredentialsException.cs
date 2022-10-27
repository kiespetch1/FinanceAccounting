namespace FinanceAccounting.Exceptions;

public class WrongCredentialsException : BaseException
{
    public WrongCredentialsException(string message)
        : base(message) { }
}