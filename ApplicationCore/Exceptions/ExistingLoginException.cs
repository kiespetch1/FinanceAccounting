using System.Net;

namespace PublicApi.Exceptions;

public class ExistingLoginException : BaseException
{
    public ExistingLoginException()
        : base("This login or email is taken. Pick another one.", HttpStatusCode.BadRequest) { }
}