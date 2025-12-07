using SourceAPI.Models;
using SourceAPI.Repositories.Interfaces;
using SourceAPI.Services.Interfaces;

namespace SourceAPI.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Product> CreateProductAsync(CreateProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price
        };

        return await _repository.CreateAsync(product);
    }

    public async Task<bool> UpdateProductAsync(int id, UpdateProductRequest request)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
        {
            return false;
        }

        product.Name = request.Name;
        product.Price = request.Price;

        await _repository.UpdateAsync(product);
        return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
        {
            return false;
        }

        await _repository.DeleteAsync(product);
        return true;
    }
}