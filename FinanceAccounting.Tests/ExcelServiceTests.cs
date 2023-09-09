using ApplicationCore.Models.SearchContexts;
using ApplicationCore.Services;
using Microsoft.IdentityModel.Tokens;
using Xunit.Sdk;

namespace FinanceAccounting.Tests;

public class ExcelServiceTests
{
    [Fact]
    public async void DataAddedToExcelFileSuccessfully()
    {
        var ctx = TestDataHelper.CreateMockDb().Object;
        var service = new ExcelService(ctx);

        var wb = await service.GetFile(1, new CashflowSearchContext());
        var incomeSheet = wb.Worksheet("Income");

        if (incomeSheet.Cell(3, 1).GetString().IsNullOrEmpty())
        {
            throw new Exception();
        }

        var incomeIdValue = incomeSheet.Cell(3, 1).GetString();
        var incomeNameValue = incomeSheet.Cell(3, 2).GetString();
        var incomeAmountValue = Convert.ToInt32(incomeSheet.Cell(3, 3).GetDouble());
        var incomeDateValue = incomeSheet.Cell(3, 4).GetDateTime();


        //var income = ctx.Income.SingleOrDefault(x => x.Id == incomeIdValue);
        //var areValuesInPlace = incomeNameValue == income.Name && incomeAmountValue == income.Amount &&
        //                       incomeDateValue == income.CreatedAt;

        //Assert.True(areValuesInPlace);
    }
}