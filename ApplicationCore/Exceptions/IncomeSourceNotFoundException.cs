using System.Net;

namespace PublicApi.Exceptions;

public class IncomeSourceNotFoundException : BaseException
{  
    public IncomeSourceNotFoundException()
        : base("Source of income could not be found.", HttpStatusCode.BadRequest) { }
}