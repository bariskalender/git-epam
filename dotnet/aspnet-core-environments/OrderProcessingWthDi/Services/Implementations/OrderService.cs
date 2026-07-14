using OrderProcessingWithDi.Models;
using OrderProcessingWithDi.Models.Exceptions;
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
        var validation = this.validator.Validate(productId, quantity, unitPrice);

        if (!validation.IsValid)
        {
            throw new InvalidOrderException(validation.ErrorMessage!);
        }

        var total = this.pricingService.CalculateTotal(unitPrice, quantity);

        var result = new OrderResult(productId, quantity, total);

        await this.repository.SaveAsync(result);

        return result;
    }
}
