using System.Net;

namespace PublicApi.Exceptions;

public class NotFoundException : BaseException
{  
    public NotFoundException()
        : base("This item could not be found.", HttpStatusCode.BadRequest) { }
}