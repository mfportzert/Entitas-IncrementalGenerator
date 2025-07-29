using System.Collections.Immutable;
using Entitas.CodeGeneration.Components.Data;
using Entitas.CodeGeneration.Extensions;

namespace Entitas.CodeGeneration.Components.Extensions;

public static class MemberDataExtensions
{
    public static string GetMethodParameters(this ImmutableArray<MemberData> memberData, bool newPrefix) 
        => string.Join(", ", memberData.Select(info 
            => info.Type + (newPrefix ? $" new{info.Name.ToUpperFirst()}" : $" {info.Name.ToLowerFirst()}")));

    public static string GetMethodArgs(this ImmutableArray<MemberData> memberData, bool newPrefix)
        => string.Join(", ", memberData.Select(info 
            => newPrefix ? $"new{info.Name.ToUpperFirst()}" : info.Name));
    
    public static string GetMemberAssignmentList(this ImmutableArray<MemberData> memberData) 
        => string.Join("\n", memberData.Select(info
            => $"        component.{info.Name} = new{info.Name.ToUpperFirst()};"));
}