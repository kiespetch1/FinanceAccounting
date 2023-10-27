using ApplicationCore.Models;
using ApplicationCore.Models.SearchContexts;
using ApplicationCore.Services;
using ApplicationCore.Utils;
using Entities.Entities;

namespace FinanceAccounting.Tests;

public class IncomeServiceTest
{
    [Fact]
    public async void AllDataFromDbIsRetrievedSuccessfully()
    {
        var ctx = TestDataHelper.CreateMockDb().Object;
        var service = new IncomeService(ctx);

        var response = await service.GetList(1,
            new CashflowSearchContext(DateTime.MinValue, DateTime.MaxValue,
                new CashflowFilter(null, null, null)),
            new PaginationContext(0), CashflowSort.NameAsc);

        var expectedList = TestDataHelper.GetIncomeMock();

        var expectedResponse = new TypeResponse<Income>
        {
            Items = expectedList,
            Total = expectedList.Count
        };

        Assert.Equal(expectedResponse, response, new TypeResponseComparer<Income>());
    }
}