using System.Net;

namespace FinanceAccounting.Exceptions;

public class ExistingIncomeSourceException : BaseException
{
    public ExistingIncomeSourceException()
        : base("Income source with given name already exists. Pick another name.", HttpStatusCode.BadRequest) { }
}