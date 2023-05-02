using System.Net;

namespace PublicApi.Exceptions;

public class ExpenseNotFoundException: BaseException
{  
    public ExpenseNotFoundException()
        : base("The expense could not be found.", HttpStatusCode.BadRequest) { }
}
