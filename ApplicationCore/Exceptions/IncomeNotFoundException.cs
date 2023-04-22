using System.Net;

namespace FinanceAccounting.Exceptions;

public class IncomeNotFoundException : BaseException
{  
    public IncomeNotFoundException()
        : base("The income could not be found.", HttpStatusCode.BadRequest) { }
}