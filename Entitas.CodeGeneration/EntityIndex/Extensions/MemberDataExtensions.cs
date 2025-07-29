using Entitas.CodeGeneration.Attributes;
using Entitas.CodeGeneration.Components.Data;

namespace Entitas.CodeGeneration.EntityIndex.Extensions;

public static class MemberDataExtensions
{
    public static string GetEntityIndexType(this MemberData memberData)
    {
        if (!memberData.IsEntityIndex)
            return string.Empty;
        
        return memberData.EntityIndexType switch
        {
            EntityIndexType.PrimaryEntityIndex => EntityIndexGenerationHelper.PrimaryEntityIndexTypeName,
            EntityIndexType.EntityIndex => EntityIndexGenerationHelper.EntityIndexTypeName,
            _ => string.Empty,
        };
    }
}