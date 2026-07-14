using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Data;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<CustomerModel>> GetAllAsync()
    {
        var customers =
            await this.unitOfWork.CustomerRepository.GetAllWithDetailsAsync();

        return this.mapper.Map<IEnumerable<CustomerModel>>(customers);
    }

    public async Task<CustomerModel> GetByIdAsync(int id)
    {
        var customer =
            await this.unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(id);

        if (customer == null)
        {
            throw new MarketException();
        }

        return this.mapper.Map<CustomerModel>(customer);
    }

    public async Task<CustomerModel> AddAsync(CustomerModel model)
    {
        if (model == null)
        {
            throw new MarketException();
        }

        if (string.IsNullOrWhiteSpace(model.Name) ||
            string.IsNullOrWhiteSpace(model.Surname))
        {
            throw new MarketException();
        }

        if (model.BirthDate > DateTime.UtcNow ||
            model.BirthDate.Year < 1901)
        {
            throw new MarketException();
        }

        var person = new Person
        {
            Name = model.Name,
            Surname = model.Surname,
            BirthDate = model.BirthDate,
        };

        var customer = new Customer
        {
            Id = model.Id,
            DiscountValue = model.DiscountValue,
            Person = person,
        };

        await this.unitOfWork.CustomerRepository.AddAsync(customer);

        await this.unitOfWork.SaveAsync();

        return this.mapper.Map<CustomerModel>(customer);
    }

    public async Task UpdateAsync(CustomerModel model)
    {
        if (model == null)
        {
            throw new MarketException();
        }

        if (string.IsNullOrWhiteSpace(model.Name) ||
            string.IsNullOrWhiteSpace(model.Surname))
        {
            throw new MarketException();
        }

        if (model.BirthDate > DateTime.UtcNow ||
            model.BirthDate.Year < 1901)
        {
            throw new MarketException();
        }

        var customer = new Customer
        {
            Id = model.Id,
            DiscountValue = model.DiscountValue,
        };

        this.unitOfWork.CustomerRepository.Update(customer);

        await this.unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int modelId)
    {
        await this.unitOfWork.CustomerRepository.DeleteByIdAsync(modelId);

        await this.unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
    {
        var customers =
            await this.unitOfWork.CustomerRepository.GetAllWithDetailsAsync();

        var filtered = customers.Where(x =>
            x.Receipts.Any(r =>
                r.ReceiptDetails.Any(rd => rd.ProductId == productId)));

        return this.mapper.Map<IEnumerable<CustomerModel>>(filtered);
    }
}
