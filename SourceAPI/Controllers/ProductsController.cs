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
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll([FromQuery] ProductQueryParameters parameters)
    {
        // フィルタ条件がない場合はnullを渡して全件取得
        if (string.IsNullOrEmpty(parameters.Name) && !parameters.MinPrice.HasValue && !parameters.MaxPrice.HasValue)
        {
            var allProducts = await _service.GetProductsAsync();
            return Ok(allProducts);
        }

        // フィルタ条件がある場合はExpressionを使用
        var products = await _service.GetProductsAsync(p =>
            (string.IsNullOrEmpty(parameters.Name) || p.Name.Contains(parameters.Name)) &&
            (!parameters.MinPrice.HasValue || p.Price >= parameters.MinPrice.Value) &&
            (!parameters.MaxPrice.HasValue || p.Price <= parameters.MaxPrice.Value)
        );
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponseDto>> GetById(int id)
    {
        var product = await _service.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound(new { Message = $"Product with ID {id} not found." });
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> Create(CreateProductRequest request)
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