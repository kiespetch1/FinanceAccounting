using System.Net;

namespace ApplicationCore.Exceptions;

public class UserNotFoundException : BaseException
{
    public UserNotFoundException()
        : base("The user could not be found.", HttpStatusCode.BadRequest) { }
}