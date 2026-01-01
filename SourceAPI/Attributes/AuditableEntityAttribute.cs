namespace SourceAPI.Attributes;

/// <summary>
/// このクラスに監査フィールドとヘルパーメソッドを自動生成するマーカー属性
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class AuditableEntityAttribute : Attribute
{
}