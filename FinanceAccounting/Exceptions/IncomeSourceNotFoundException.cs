using System.Net;

namespace FinanceAccounting.Exceptions;

public class IncomeSourceNotFoundException : BaseException
{  
    public IncomeSourceNotFoundException()
        : base("The income source could not be found.", HttpStatusCode.BadRequest) { }
}