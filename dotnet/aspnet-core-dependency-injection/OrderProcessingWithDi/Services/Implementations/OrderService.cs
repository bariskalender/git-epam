using OrderProcessingWithDi.Models;
using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly IPricingService pricingService;
    private readonly IOrderRepository repository;
    private readonly IOrderValidator validator;

    public OrderService(
        IPricingService pricingService,
        IOrderRepository repository,
        IOrderValidator validator)
    {
        this.pricingService = pricingService;
        this.repository = repository;
        this.validator = validator;
    }

    public async Task<OrderResult> ProcessOrderAsync(
        string productId,
        int quantity,
        decimal unitPrice)
    {
        var validationResult = this.validator.Validate(
            productId,
            quantity,
            unitPrice);

        if (!validationResult.IsValid)
        {
            throw new ArgumentException(
                validationResult.ErrorMessage);
        }

        decimal total = this.pricingService.CalculateTotal(
            unitPrice,
            quantity);

        var result = new OrderResult(
            productId,
            quantity,
            total);

        await this.repository.SaveAsync(result);

        return result;
    }
}
