using System.Net;

namespace FinanceAccounting.Exceptions;

public class WrongCredentialsException : BaseException
{
    public WrongCredentialsException()
        : base(message, ErrorCode) { }

    private new const HttpStatusCode ErrorCode = HttpStatusCode.BadRequest;
    public const string message = "Wrong password or login.";
}