using ApplicationCore.Models;
using ApplicationCore.Models.SearchContexts;
using ApplicationCore.Services;
using Entities.Entities;

namespace FinanceAccounting.Tests;

public class ExpenseServiceTest
{
    [Fact]
    public async void AllDataFromDbIsRetrievedSuccessfully()
    {
        var ctx = TestDataHelper.CreateMockDb().Object;
        var service = new ExpenseService(ctx);

        var response = await service.GetList(1,
            new CashflowSearchContext(DateTime.MinValue, DateTime.MaxValue,
                new CashflowFilter(null, null, null)),
            new PaginationContext(0), CashflowSort.NameAsc);

        var expectedList = TestDataHelper.GetExpenseMock();
        
        var expectedResponse = new TypeResponse<Expense>
        {
            Items = expectedList,
            Total = expectedList.Count
        };
        
        Assert.Equal(expectedResponse, response, new TestDataHelper.TypeResponseComparer<Expense>());
        
    }
}