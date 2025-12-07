using System.Linq.Expressions;
using SourceAPI.Models;

namespace SourceAPI.Repositories.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAsync(Expression<Func<Product, bool>>? predicate = null);
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
    Task<bool> ExistsAsync(int id);
}