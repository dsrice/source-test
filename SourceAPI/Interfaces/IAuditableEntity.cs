namespace SourceAPI.Interfaces;

/// <summary>
/// 監査可能なエンティティのインターフェース
/// [AuditableEntity]属性を使用すると自動的に実装される
/// </summary>
public interface IAuditableEntity
{
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
}