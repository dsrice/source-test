namespace SourceAPI.Models.DB;

/// <summary>
/// すべてのエンティティの基底クラス
/// 監査フィールドを含む
/// </summary>
public abstract class BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
