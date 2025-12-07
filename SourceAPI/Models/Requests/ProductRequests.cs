namespace SourceAPI.Models.Requests;

public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public class UpdateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}