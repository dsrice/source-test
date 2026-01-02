namespace SourceAPI.Attributes;

/// <summary>
/// このDBモデルに対してRepositoryインターフェースと実装クラスを自動生成するマーカー属性
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class GenerateRepositoryAttribute : Attribute
{
}
