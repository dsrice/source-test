using Microsoft.AspNetCore.Mvc;
using SourceAPI.Models;
using SourceAPI.Services.Interfaces;

namespace SourceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
    {
        var products = await _service.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _service.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound(new { Message = $"Product with ID {id} not found." });
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(CreateProductRequest request)
    {
        var createdProduct = await _service.CreateProductAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateProductRequest request)
    {
        var result = await _service.UpdateProductAsync(id, request);
        if (!result)
        {
            return NotFound(new { Message = $"Product with ID {id} not found." });
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _service.DeleteProductAsync(id);
        if (!result)
        {
            return NotFound(new { Message = $"Product with ID {id} not found." });
        }
        return NoContent();
    }
}