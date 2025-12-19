using System.Linq.Expressions;
using SourceAPI.Models.DB;
using SourceAPI.Models.Requests;
using SourceAPI.Models.Responses;

namespace SourceAPI.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductResponse>> GetProductsAsync(Expression<Func<Product, bool>>? predicate = null);
    Task<ProductResponse?> GetProductByIdAsync(int id);
    Task<ProductResponse> CreateProductAsync(CreateProductRequest request);
    Task<bool> UpdateProductAsync(int id, UpdateProductRequest request);
    Task<bool> DeleteProductAsync(int id);
}