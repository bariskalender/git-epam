using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<ProductModel>> GetAllAsync()
    {
        var products =
            await this.unitOfWork.ProductRepository.GetAllWithDetailsAsync();

        return this.mapper.Map<IEnumerable<ProductModel>>(products);
    }

    public async Task<ProductModel> GetByIdAsync(int id)
    {
        var product =
            await this.unitOfWork.ProductRepository.GetByIdWithDetailsAsync(id);

        if (product == null)
        {
            throw new MarketException();
        }

        return this.mapper.Map<ProductModel>(product);
    }

    public async Task<ProductModel> AddAsync(ProductModel model)
    {
        if (model == null ||
            string.IsNullOrWhiteSpace(model.ProductName) ||
            model.Price < 0)
        {
            throw new MarketException();
        }

        var product = this.mapper.Map<Product>(model);

        await this.unitOfWork.ProductRepository.AddAsync(product);

        await this.unitOfWork.SaveAsync();

        return this.mapper.Map<ProductModel>(product);
    }

    public async Task UpdateAsync(ProductModel model)
    {
        if (model == null ||
            string.IsNullOrWhiteSpace(model.ProductName) ||
            model.Price < 0)
        {
            throw new MarketException();
        }

        var product = this.mapper.Map<Product>(model);

        this.unitOfWork.ProductRepository.Update(product);

        await this.unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int modelId)
    {
        await this.unitOfWork.ProductRepository.DeleteByIdAsync(modelId);

        await this.unitOfWork.SaveAsync();
    }

    public Task<IEnumerable<ProductModel>> GetByFilterAsync(
        FilterSearchModel filterSearch)
    {
        ArgumentNullException.ThrowIfNull(filterSearch);

        return this.GetByFilterInternalAsync(filterSearch);
    }

    public async Task<IEnumerable<CategoryModel>> GetAllProductCategoriesAsync()
    {
        var categories =
            await this.unitOfWork.CategoryRepository.GetAllAsync();

        return this.mapper.Map<IEnumerable<CategoryModel>>(categories);
    }

    public async Task AddCategoryAsync(CategoryModel categoryModel)
    {
        if (categoryModel == null ||
            string.IsNullOrWhiteSpace(categoryModel.Name))
        {
            throw new MarketException();
        }

        var category =
            this.mapper.Map<ProductCategory>(categoryModel);

        await this.unitOfWork.CategoryRepository.AddAsync(category);

        await this.unitOfWork.SaveAsync();
    }

    public async Task UpdateCategoryAsync(CategoryModel categoryModel)
    {
        if (categoryModel == null ||
            string.IsNullOrWhiteSpace(categoryModel.Name))
        {
            throw new MarketException();
        }

        var category =
            this.mapper.Map<ProductCategory>(categoryModel);

        this.unitOfWork.CategoryRepository.Update(category);

        await this.unitOfWork.SaveAsync();
    }

    public async Task RemoveCategoryAsync(int categoryId)
    {
        await this.unitOfWork.CategoryRepository.DeleteByIdAsync(categoryId);

        await this.unitOfWork.SaveAsync();
    }

    private async Task<IEnumerable<ProductModel>> GetByFilterInternalAsync(
        FilterSearchModel filterSearch)
    {
        var products =
            await this.unitOfWork.ProductRepository.GetAllWithDetailsAsync();

        if (filterSearch.CategoryId.HasValue)
        {
            products = products.Where(x =>
                x.ProductCategoryId == filterSearch.CategoryId.Value);
        }

        if (filterSearch.MinPrice.HasValue)
        {
            products = products.Where(x =>
                x.Price >= filterSearch.MinPrice.Value);
        }

        if (filterSearch.MaxPrice.HasValue)
        {
            products = products.Where(x =>
                x.Price <= filterSearch.MaxPrice.Value);
        }

        return this.mapper.Map<IEnumerable<ProductModel>>(products);
    }
}
