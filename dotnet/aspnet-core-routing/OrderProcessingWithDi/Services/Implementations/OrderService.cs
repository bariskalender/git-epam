using OrderProcessingWithDi.Models;
using OrderProcessingWithDi.Services.Interfaces;

namespace OrderProcessingWithDi.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly IPricingService _pricing;
    private readonly IOrderRepository _repository;
    private readonly IOrderValidator _validator;

    public OrderService(
        IPricingService pricing,
        IOrderRepository repository,
        IOrderValidator validator)
    {
        _pricing = pricing;
        _repository = repository;
        _validator = validator;
    }

    public async Task<OrderResult> ProcessOrderAsync(string productId, int quantity, decimal unitPrice)
    {
        var (isValid, error) = _validator.Validate(productId, quantity, unitPrice);

        if (!isValid)
            throw new ArgumentException(error);

        var total = _pricing.CalculateTotal(unitPrice, quantity);

        var result = new OrderResult(productId, quantity, total);

        await _repository.SaveAsync(result);

        return result;
    }
}
