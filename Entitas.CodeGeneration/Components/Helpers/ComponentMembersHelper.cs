using System.Collections.Immutable;
using Entitas.CodeGeneration.Components.Data;
using Entitas.CodeGeneration.Extensions;
using Microsoft.CodeAnalysis;

namespace Entitas.CodeGeneration.Components.Helpers;

public static class ComponentMembersHelper
{
    public static ImmutableArray<MemberData> GetComponentMembers(INamedTypeSymbol type)
    {
        var publicMembersSymbols = type.GetPublicMembers(true);
        
        var membersDataBuilder = ImmutableArray.CreateBuilder<MemberData>();
        foreach (var memberSymbol in publicMembersSymbols)
        {
            membersDataBuilder.Add(new MemberData(memberSymbol));
        }

        return membersDataBuilder.ToImmutable();
    }
}