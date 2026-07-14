using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;

namespace Business.Services;

public class StatisticService : IStatisticService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public StatisticService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
    {
        var details = await this.unitOfWork.ReceiptDetailRepository.GetAllWithDetailsAsync();

        var products = details
            .GroupBy(x => x.Product)
            .OrderByDescending(x => x.Sum(d => d.Quantity))
            .Take(productCount)
            .Select(x => x.Key);

        return this.mapper.Map<IEnumerable<ProductModel>>(products);
    }

    public async Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId)
    {
        var receipts = await this.unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

        var products = receipts
            .Where(x => x.CustomerId == customerId)
            .SelectMany(x => x.ReceiptDetails)
            .GroupBy(x => x.Product)
            .OrderByDescending(x => x.Sum(d => d.Quantity))
            .Take(productCount)
            .Select(x => x.Key);

        return this.mapper.Map<IEnumerable<ProductModel>>(products);
    }

    public async Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(
        int customerCount,
        DateTime startDate,
        DateTime endDate)
    {
        var receipts = await this.unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

        var customers = receipts
            .Where(x =>
                x.OperationDate >= startDate &&
                x.OperationDate <= endDate)
            .GroupBy(x => x.Customer)
            .Select(x => new CustomerActivityModel
            {
                CustomerId = x.Key.Id,
                CustomerName = $"{x.Key.Person.Name} {x.Key.Person.Surname}",
                ReceiptSum = x.Sum(r =>
                    r.ReceiptDetails.Sum(d => d.DiscountUnitPrice * d.Quantity)),
            })
            .OrderByDescending(x => x.ReceiptSum)
            .Take(customerCount);

        return customers;
    }

    public async Task<decimal> GetIncomeOfCategoryInPeriod(
        int categoryId,
        DateTime startDate,
        DateTime endDate)
    {
        var receipts = await this.unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

        return receipts
            .Where(x =>
                x.OperationDate >= startDate &&
                x.OperationDate <= endDate)
            .SelectMany(x => x.ReceiptDetails)
            .Where(x => x.Product.ProductCategoryId == categoryId)
            .Sum(x => x.DiscountUnitPrice * x.Quantity);
    }
}
