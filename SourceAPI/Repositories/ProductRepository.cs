using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SourceAPI.Data;
using SourceAPI.Models;
using SourceAPI.Repositories.Interfaces;

namespace SourceAPI.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAsync(Expression<Func<Product, bool>>? predicate = null)
    {
        if (predicate == null)
        {
            return await _context.Products.ToListAsync();
        }

        return await _context.Products.Where(predicate).ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        // 論理削除（ソフトデリート）
        product.IsDeleted = true;
        product.DeletedAt = DateTime.UtcNow;
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Products.AnyAsync(p => p.Id == id);
    }
}