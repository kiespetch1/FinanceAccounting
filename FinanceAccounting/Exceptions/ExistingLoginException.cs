namespace FinanceAccounting.Exceptions;

public class ExistingLoginException : Exception
{
    public ExistingLoginException(string message)
        : base(message) { }
}