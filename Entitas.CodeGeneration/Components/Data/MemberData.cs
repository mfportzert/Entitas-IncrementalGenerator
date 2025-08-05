using Entitas.CodeGeneration.EntityIndex;
using Entitas.CodeGeneration.Extensions;
using Microsoft.CodeAnalysis;

namespace Entitas.CodeGeneration.Components.Data;

public readonly struct MemberData : IEquatable<MemberData>
{
    public string Type { get; }
    public string Name { get; }
    public bool IsEntityIndex { get; }
    public EntityIndexType EntityIndexType { get; }

    public MemberData(ISymbol symbol)
    {
        Type = symbol.GetPublicMemberType().ToCompilableString();
        Name = symbol.Name;
        
        IsEntityIndex = EntityIndexGenerationHelper.TryFindEntityIndexType(symbol, 
            out EntityIndexType entityIndexType);
        
        EntityIndexType = entityIndexType;
    }
    
    public MemberData(string type, string name, 
        bool isEntityIndex = false, 
        EntityIndexType entityIndexType = default)
    {
        Type = type;
        Name = name;
        
        IsEntityIndex = isEntityIndex;
        EntityIndexType = entityIndexType;
    }

    public bool Equals(MemberData other)
    {
        return string.Equals(Type, other.Type, StringComparison.Ordinal) &&
               string.Equals(Name, other.Name, StringComparison.Ordinal) &&
               IsEntityIndex == other.IsEntityIndex && 
               EntityIndexType == other.EntityIndexType;
    }

    public override bool Equals(object? obj) =>
        obj is MemberData other && Equals(other);

    public override int GetHashCode()
    {
        unchecked // Allow arithmetic overflow (doesn't throw)
        {
            int hash = 17;
            hash = hash * 31 + (Type?.GetHashCode() ?? 0);
            hash = hash * 31 + (Name?.GetHashCode() ?? 0);
            hash = hash * 31 + IsEntityIndex.GetHashCode();
            hash = hash * 31 + EntityIndexType.GetHashCode();
            return hash;
        }
    }
    
    public static bool operator ==(MemberData left, MemberData right) => left.Equals(right);
    public static bool operator !=(MemberData left, MemberData right) => !left.Equals(right);
}

public enum EntityIndexType
{
    EntityIndex,
    PrimaryEntityIndex
}