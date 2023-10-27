using ApplicationCore.Models;
using ApplicationCore.Models.SearchContexts;
using ApplicationCore.Services;
using ApplicationCore.Utils;
using Entities.Entities;

namespace FinanceAccounting.Tests;

public class IncomeSourceServiceTest
{
    [Fact]
    public async void AllDataFromDbIsRetrievedSuccessfully()
    {
        var ctx = TestDataHelper.CreateMockDb().Object;
        var service = new IncomeSourceService(ctx);

        var response = await service.GetList(1, new PaginationContext(page: 1), CategoriesSort.NameAsc,
            new CategoriesFilter());

        var expectedList = TestDataHelper.GetIncomeSourcesMock();

        var expectedResponse = new TypeResponse<IncomeSource>
        {
            Items = expectedList,
            Total = expectedList.Count
        };
        
        Assert.Equal(expectedResponse, response, new TypeResponseComparer<IncomeSource>());

    }
}