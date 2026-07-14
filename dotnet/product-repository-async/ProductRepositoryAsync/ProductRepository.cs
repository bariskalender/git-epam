using System.Globalization;

namespace ProductRepositoryAsync;

/// <summary>
/// Represents a product storage service and provides a set of methods for managing the list of products.
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly string productCollectionName;
    private readonly IDatabase database;

    public ProductRepository(string productCollectionName, IDatabase database)
    {
        this.productCollectionName = productCollectionName;
        this.database = database;
    }

    public int AddProduct(Product product)
    {
        throw new NotSupportedException();
    }

    public Product GetProduct(int productId)
    {
        throw new NotSupportedException();
    }

    public void RemoveProduct(int productId)
    {
        throw new NotSupportedException();
    }

    public void UpdateProduct(Product product)
    {
        throw new NotSupportedException();
    }

    public async Task<Product> GetProductAsync(int productId)
    {
        OperationResult result = await this.database.IsCollectionExistAsync(this.productCollectionName, out bool collectionExists);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }

        if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        if (!collectionExists)
        {
            throw new CollectionNotFoundException();
        }

        result = await this.database.IsCollectionElementExistAsync(this.productCollectionName, productId, out bool collectionElementExists);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }

        if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        if (!collectionElementExists)
        {
            throw new ProductNotFoundException();
        }

        result = await this.database.GetCollectionElementAsync(this.productCollectionName, productId, out IDictionary<string, string> data);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }

        if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        return new Product
        {
            Id = productId,
            Name = data["name"],
            Category = data["category"],
            UnitPrice = decimal.Parse(data["price"], CultureInfo.InvariantCulture),
            UnitsInStock = int.Parse(data["in-stock"], CultureInfo.InvariantCulture),
            Discontinued = bool.Parse(data["discontinued"]),
        };
    }

    public Task<int> AddProductAsync(Product product)
    {
        ValidateProduct(product);
        return this.AddProductInternalAsync(product);
    }

    public async Task RemoveProductAsync(int productId)
    {
        OperationResult result = await this.database.IsCollectionExistAsync(this.productCollectionName, out bool collectionExists);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }

        if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        if (!collectionExists)
        {
            throw new CollectionNotFoundException();
        }

        result = await this.database.IsCollectionElementExistAsync(this.productCollectionName, productId, out bool collectionElementExists);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }

        if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        if (!collectionElementExists)
        {
            throw new ProductNotFoundException();
        }

        result = await this.database.DeleteCollectionElementAsync(this.productCollectionName, productId);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }

        if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }
    }

    public Task UpdateProductAsync(Product product)
    {
        ValidateProduct(product);
        return this.UpdateProductInternalAsync(product);
    }

    private async Task<int> AddProductInternalAsync(Product product)
    {
        OperationResult result = await this.database.IsCollectionExistAsync(this.productCollectionName, out bool collectionExists);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }

        if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        if (!collectionExists)
        {
            result = await this.database.CreateCollectionAsync(this.productCollectionName);

            if (result == OperationResult.ConnectionIssue)
            {
                throw new DatabaseConnectionException();
            }

            if (result != OperationResult.Success)
            {
                throw new RepositoryException();
            }
        }

        result = await this.database.GenerateIdAsync(this.productCollectionName, out int productId);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }

        if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        IDictionary<string, string> data = new Dictionary<string, string>
        {
            ["name"] = product.Name,
            ["category"] = product.Category,
            ["price"] = product.UnitPrice.ToString(CultureInfo.InvariantCulture),
            ["in-stock"] = product.UnitsInStock.ToString(CultureInfo.InvariantCulture),
            ["discontinued"] = product.Discontinued.ToString(CultureInfo.InvariantCulture),
        };

        result = await this.database.InsertCollectionElementAsync(this.productCollectionName, productId, data);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }

        if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        return productId;
    }

    private async Task UpdateProductInternalAsync(Product product)
    {
        OperationResult result = await this.database.IsCollectionExistAsync(this.productCollectionName, out bool collectionExists);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }

        if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        if (!collectionExists)
        {
            throw new CollectionNotFoundException();
        }

        result = await this.database.IsCollectionElementExistAsync(this.productCollectionName, product.Id, out bool collectionElementExists);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }

        if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }

        if (!collectionElementExists)
        {
            throw new ProductNotFoundException();
        }

        IDictionary<string, string> data = new Dictionary<string, string>
        {
            ["name"] = product.Name,
            ["category"] = product.Category,
            ["price"] = product.UnitPrice.ToString(CultureInfo.InvariantCulture),
            ["in-stock"] = product.UnitsInStock.ToString(CultureInfo.InvariantCulture),
            ["discontinued"] = product.Discontinued.ToString(CultureInfo.InvariantCulture),
        };

        result = await this.database.UpdateCollectionElementAsync(this.productCollectionName, product.Id, data);

        if (result == OperationResult.ConnectionIssue)
        {
            throw new DatabaseConnectionException();
        }

        if (result != OperationResult.Success)
        {
            throw new RepositoryException();
        }
    }

    private static void ValidateProduct(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (string.IsNullOrWhiteSpace(product.Name) ||
            string.IsNullOrWhiteSpace(product.Category) ||
            product.UnitPrice < 0 ||
            product.UnitsInStock < 0)
        {
            throw new ArgumentException("Invalid product data.", nameof(product));
        }
    }
}
