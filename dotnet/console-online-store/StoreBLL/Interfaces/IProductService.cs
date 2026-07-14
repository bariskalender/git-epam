namespace StoreBLL.Interfaces;

using StoreBLL.Models;
using System.Collections.Generic;

public interface IProductService
{
    IEnumerable<ProductModel> GetAllProducts();
}