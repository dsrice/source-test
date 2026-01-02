using Mapster;
using SourceAPI.Models.DB;
using SourceAPI.Models.Responses;

namespace SourceAPI.Mappings;

/// <summary>
/// Mapsterのマッピング設定
/// </summary>
public static class MappingConfig
{
    /// <summary>
    /// マッピング設定を登録
    /// </summary>
    public static void RegisterMappings()
    {
        // Product -> ProductResponse
        // プロパティ名が一致しているため、明示的な設定は不要（Mapsterが自動マッピング）
        TypeAdapterConfig<Product, ProductResponse>.NewConfig();

        // User -> UserResponse など、将来的に追加するマッピングもここに記述
    }
}
