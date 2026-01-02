using System.Linq.Expressions;
using SourceAPI.Models.DB;
using SourceAPI.Models.Requests;
using SourceAPI.Models.Responses;
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

    public async Task<IEnumerable<ProductResponse>> GetProductsAsync(Expression<Func<Product, bool>>? predicate = null)
    {
        var products = await _repository.GetAsync(predicate);
        if (products == null)
        {
            throw new InvalidOperationException("商品の取得に失敗しました");
        }
        return products.Select(MapToDto);
    }

    public async Task<ProductResponse?> GetProductByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        return product == null ? null : MapToDto(product);
    }

    public async Task<ProductResponse> CreateProductAsync(CreateProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price
        };

        var success = await _repository.CreateAsync(product);
        if (!success)
        {
            throw new InvalidOperationException("商品の作成に失敗しました");
        }

        return MapToDto(product);
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

        return await _repository.UpdateAsync(product);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
        {
            return false;
        }

        return await _repository.DeleteAsync(product);
    }

    private static ProductResponse MapToDto(Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }
}