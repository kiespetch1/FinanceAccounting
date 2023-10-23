using ApplicationCore.Models;
using ApplicationCore.Models.SearchContexts;
using ApplicationCore.Services;
using DocumentFormat.OpenXml.Wordprocessing;
using Entities.Entities;

namespace FinanceAccounting.Tests;

public class ExpenseSourceServiceTest
{
    [Fact]
    public async void AllDataFromDbIsRetrievedSuccessfully()
    {
        var ctx = TestDataHelper.CreateMockDb().Object;
        var service = new ExpenseSourceService(ctx);

        var response = await service.GetList(1, new PaginationContext(page: 1), CategoriesSort.NameAsc,
            new CategoriesFilter());

        var expectedList = TestDataHelper.GetExpenseSourcesMock();

        var expectedResponse = new TypeResponse<ExpenseSource>
        {
            Items = expectedList,
            Total = expectedList.Count
        };
        
        Assert.Equal(expectedResponse, response, new TestDataHelper.TypeResponseComparer<ExpenseSource>());

    }
}