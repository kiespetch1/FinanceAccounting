using ApplicationCore.Models.SearchContexts;
using ApplicationCore.Services;

namespace FinanceAccounting.Tests;

public class ExcelServiceTests
{
    [Fact]
    public async void DataAddedToExcelFileSuccessfully()
    {
        var ctx = TestDataHelper.CreateMockDb().Object;
        var service = new ExcelService(ctx);

        var wb = await service.GetFile(1, new CashflowSearchContext
        {
            From = new DateTime(1,1,1),
            To = new DateTime(3000,12,31),
            CashflowFilter = null
        });
        var incomeSheet = wb.Worksheet("Income");

        var incomeIdValue = Convert.ToInt32(incomeSheet.Cell(3, 1).GetString());
        var incomeNameValue = incomeSheet.Cell(3, 2).GetString();
        var incomeAmountValue = Convert.ToDecimal(incomeSheet.Cell(3, 3).GetDouble());
        var incomeDateValue = incomeSheet.Cell(3, 4).GetDateTime();
        
        var income = ctx.Income.SingleOrDefault(x => x.Id == incomeIdValue);
        
        var namesAreIdentical = incomeNameValue == income.Name;
        var amountsAreIdentical = incomeAmountValue == income.Amount;
        var datesAreIdentical = TestDataHelper.DatesAreEqualIgnoringMilliseconds(incomeDateValue, income.CreatedAt);


        var areValuesInPlace = namesAreIdentical && amountsAreIdentical && datesAreIdentical;

        Assert.True(areValuesInPlace);
    }
}