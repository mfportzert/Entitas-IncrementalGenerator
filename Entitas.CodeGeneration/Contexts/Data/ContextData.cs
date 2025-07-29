using Entitas.CodeGeneration.Components;
using Entitas.CodeGeneration.Components.Extensions;

namespace Entitas.CodeGeneration.Contexts.Data;

public readonly struct ContextData : IEquatable<ContextData>
{
    public string ContextName { get; } // ex: Game
    public string ContextTypeName { get; } // ex: GameContext
    public string MatcherTypeName { get; } // ex: GameMatcher
    public string EntityTypeName { get; } // ex: GameEntity
    public string SystemTypeName { get; } // ex: GameSystem
    public string ComponentsLookupTypeName { get; } // ex: GameComponentsLookup
    
    public ContextData(string name)
    {
        ContextName = name;
        ContextTypeName = name.AddContextSuffix();
        MatcherTypeName = name.AddMatcherSuffix();
        EntityTypeName = name.AddEntitySuffix();
        SystemTypeName = name.AddSystemSuffix();
        ComponentsLookupTypeName = name + ComponentGenerationHelper.ComponentsLookupName;
    }
    
    public bool Equals(ContextData other) =>
        string.Equals(ContextName, other.ContextName, StringComparison.Ordinal);

    public override bool Equals(object? obj) =>
        obj is ContextData other && Equals(other);

    public override int GetHashCode() =>
        ContextName != null ? StringComparer.Ordinal.GetHashCode(ContextName) : 0;
    
    public static bool operator ==(ContextData left, ContextData right) => left.Equals(right);
    public static bool operator !=(ContextData left, ContextData right) => !left.Equals(right);
}