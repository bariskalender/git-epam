using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class ReceiptService : IReceiptService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public ReceiptService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
    {
        var receipts = await this.unitOfWork.ReceiptRepository.GetAllAsync();
        return this.mapper.Map<IEnumerable<ReceiptModel>>(receipts);
    }

    public async Task<ReceiptModel> GetByIdAsync(int id)
    {
        var receipts = await this.unitOfWork.ReceiptRepository.GetAllAsync();

        var receipt = receipts.FirstOrDefault(x => x.Id == id)
            ?? throw new MarketException("Receipt not found");

        return this.mapper.Map<ReceiptModel>(receipt);
    }

    public async Task<ReceiptModel> AddAsync(ReceiptModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var entity = this.mapper.Map<Receipt>(model);

        await this.unitOfWork.ReceiptRepository.AddAsync(entity);
        await this.unitOfWork.SaveAsync();

        return this.mapper.Map<ReceiptModel>(entity);
    }

    public async Task UpdateAsync(ReceiptModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var receipts = await this.unitOfWork.ReceiptRepository.GetAllAsync();

        var entity = receipts.FirstOrDefault(x => x.Id == model.Id)
            ?? throw new MarketException("Receipt not found");

        entity.CustomerId = model.CustomerId;
        entity.OperationDate = model.OperationDate;
        entity.IsCheckedOut = model.IsCheckedOut;

        this.unitOfWork.ReceiptRepository.Update(entity);
        await this.unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int modelId)
    {
        var details = await this.unitOfWork.ReceiptDetailRepository.GetAllAsync();

        var related = details.Where(x => x.ReceiptId == modelId).ToList();

        foreach (var item in related)
        {
            this.unitOfWork.ReceiptDetailRepository.Delete(item);
        }

        await this.unitOfWork.ReceiptRepository.DeleteByIdAsync(modelId);
        await this.unitOfWork.SaveAsync();
    }

    public async Task AddProductAsync(int productId, int receiptId, int quantity)
    {
        var receipts = await this.unitOfWork.ReceiptRepository.GetAllAsync();
        var products = await this.unitOfWork.ProductRepository.GetAllAsync();
        var details = await this.unitOfWork.ReceiptDetailRepository.GetAllAsync();

        var receipt = receipts.FirstOrDefault(x => x.Id == receiptId)
            ?? throw new MarketException("Receipt not found");

        var product = products.FirstOrDefault(x => x.Id == productId)
            ?? throw new MarketException("Product not found");

        var existing = details.FirstOrDefault(x =>
            x.ProductId == productId && x.ReceiptId == receiptId);

        if (existing != null)
        {
            existing.Quantity += quantity;
            this.unitOfWork.ReceiptDetailRepository.Update(existing);
        }
        else
        {
            var discount = receipt.Customer.DiscountValue;

            var detail = new ReceiptDetail
            {
                ProductId = productId,
                ReceiptId = receiptId,
                Quantity = quantity,
                UnitPrice = product.Price,
                DiscountUnitPrice =
                    product.Price - (product.Price * discount / 100m)
            };

            await this.unitOfWork.ReceiptDetailRepository.AddAsync(detail);
        }

        await this.unitOfWork.SaveAsync();
    }

    public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
    {
        var details = await this.unitOfWork.ReceiptDetailRepository.GetAllAsync();

        var detail = details.FirstOrDefault(x =>
            x.ProductId == productId && x.ReceiptId == receiptId)
            ?? throw new MarketException("Receipt detail not found");

        detail.Quantity -= quantity;

        if (detail.Quantity <= 0)
        {
            this.unitOfWork.ReceiptDetailRepository.Delete(detail);
        }
        else
        {
            this.unitOfWork.ReceiptDetailRepository.Update(detail);
        }

        await this.unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
    {
        var details = await this.unitOfWork.ReceiptDetailRepository.GetAllAsync();

        var result = details.Where(x => x.ReceiptId == receiptId);

        return this.mapper.Map<IEnumerable<ReceiptDetailModel>>(result);
    }

    public async Task<decimal> ToPayAsync(int receiptId)
    {
        var details = await this.unitOfWork.ReceiptDetailRepository.GetAllAsync();

        return details
            .Where(x => x.ReceiptId == receiptId)
            .Sum(x => x.Quantity * x.DiscountUnitPrice);
    }

    public async Task CheckOutAsync(int receiptId)
    {
        var receipts = await this.unitOfWork.ReceiptRepository.GetAllAsync();

        var receipt = receipts.FirstOrDefault(x => x.Id == receiptId)
            ?? throw new MarketException("Receipt not found");

        receipt.IsCheckedOut = true;

        this.unitOfWork.ReceiptRepository.Update(receipt);
        await this.unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(
        DateTime startDate,
        DateTime endDate)
    {
        var receipts = await this.unitOfWork.ReceiptRepository.GetAllAsync();

        var result = receipts.Where(x =>
            x.OperationDate >= startDate &&
            x.OperationDate <= endDate);

        return this.mapper.Map<IEnumerable<ReceiptModel>>(result);
    }
}
