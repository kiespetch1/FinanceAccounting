using System.Net;

namespace FinanceAccounting.Exceptions;

public class ExistingLoginException : BaseException
{
    public ExistingLoginException()
        : base(message, ErrorCode) { }

    private new const HttpStatusCode ErrorCode = HttpStatusCode.BadRequest;
    private const string message = "This login or email is taken. Pick another one.";
}