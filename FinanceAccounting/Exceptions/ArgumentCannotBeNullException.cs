using System.Net;

namespace FinanceAccounting.Exceptions;

public class ArgumentCannotBeNullException : BaseException
{
    public ArgumentCannotBeNullException()
        : base("New data cannot be empty or the same as old.", HttpStatusCode.BadRequest) { }
}