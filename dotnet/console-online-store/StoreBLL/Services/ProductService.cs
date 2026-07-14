namespace StoreBLL.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using StoreBLL.Interfaces;
    using StoreBLL.Models;
    using StoreDAL.Interfaces;

    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public IEnumerable<ProductModel> GetAllProducts()
        {
            return this.productRepository
                .GetAll()
                .Select(x => new ProductModel
                {
                    Id = x.Id,
                    TitleId = x.TitleId,
                    ManufacturerId = x.ManufacturerId,
                    Description = x.Description ?? string.Empty,
                    UnitPrice = x.UnitPrice,
                })
                .ToList();
        }
    }
}