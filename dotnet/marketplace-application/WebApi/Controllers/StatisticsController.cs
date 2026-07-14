using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/statistics")]
public class StatisticsController(IStatisticService statisticService) : ControllerBase
{
    [HttpGet("most-popular-products")]
    public async Task<IActionResult> GetMostPopularProducts(int count)
    {
        if (count < 0)
        {
            return this.BadRequest();
        }

        var result = await statisticService.GetMostPopularProductsAsync(count);

        return this.Ok(result);
    }

    [HttpGet("customers-most-popular-products")]
    public async Task<IActionResult> GetCustomersMostPopularProducts(
        int count,
        int customerId)
    {
        if (count < 0 || customerId < 0)
        {
            return this.BadRequest();
        }

        var result = await statisticService
            .GetCustomersMostPopularProductsAsync(count, customerId);

        return this.Ok(result);
    }

    [HttpGet("most-valuable-customers")]
    public async Task<IActionResult> GetMostValuableCustomers(
        int count,
        DateTime startDate,
        DateTime endDate)
    {
        if (startDate > endDate)
        {
            return this.BadRequest();
        }

        var result = await statisticService
            .GetMostValuableCustomersAsync(count, startDate, endDate);

        return this.Ok(result);
    }

    [HttpGet("income-of-category")]
    public async Task<IActionResult> GetIncomeOfCategory(
        int categoryId,
        DateTime startDate,
        DateTime endDate)
    {
        var result = await statisticService
            .GetIncomeOfCategoryInPeriod(categoryId, startDate, endDate);

        return this.Ok(result);
    }
}
