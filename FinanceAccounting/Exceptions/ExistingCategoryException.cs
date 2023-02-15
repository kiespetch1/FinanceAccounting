using System.Net;

namespace FinanceAccounting.Exceptions;

public class ExistingCategoryException : BaseException
{
    public ExistingCategoryException()
        : base("Category with given name already exists. Pick another name.", HttpStatusCode.BadRequest) { }
}