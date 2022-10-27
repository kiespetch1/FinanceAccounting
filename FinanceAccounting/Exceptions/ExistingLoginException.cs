namespace FinanceAccounting.Exceptions;

public class ExistingLoginException : BaseException
{
    public ExistingLoginException(string message)
        : base(message) { }
}