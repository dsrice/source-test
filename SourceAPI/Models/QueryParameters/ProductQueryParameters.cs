namespace SourceAPI.Models.QueryParameters;

public class ProductQueryParameters
{
    public string? Name { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}