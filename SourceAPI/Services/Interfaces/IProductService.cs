using System.Linq.Expressions;
using SourceAPI.Models;

namespace SourceAPI.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductResponseDto>> GetProductsAsync(Expression<Func<Product, bool>>? predicate = null);
    Task<ProductResponseDto?> GetProductByIdAsync(int id);
    Task<ProductResponseDto> CreateProductAsync(CreateProductRequest request);
    Task<bool> UpdateProductAsync(int id, UpdateProductRequest request);
    Task<bool> DeleteProductAsync(int id);
}