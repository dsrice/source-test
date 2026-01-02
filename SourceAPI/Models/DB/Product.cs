using SourceAPI.Attributes;

namespace SourceAPI.Models.DB;

[AuditableEntity]
[GenerateRepository]
public partial class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}