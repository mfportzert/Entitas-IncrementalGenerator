using System.Collections.Immutable;
using Entitas.CodeGeneration.Attributes;
using Entitas.CodeGeneration.Components.Extensions;
using Entitas.CodeGeneration.Components.Helpers;
using Entitas.CodeGeneration.Contexts.Data;
using Entitas.CodeGeneration.Extensions;
using Microsoft.CodeAnalysis;

namespace Entitas.CodeGeneration.Components.Data;

public readonly struct ComponentData : IEquatable<ComponentData>
{
    public string ShortComponentName { get; } // ex: Position3
    public string FullComponentName { get; } // ex: MyNamespacePosition3

    public string ShortTypeName { get; } // ex: Position3Component
    public string FullTypeName { get; } // ex: MyNamespace.Position3Component
    
    // Attributes
    public ImmutableArray<string> ContextNames { get; }
    public ImmutableArray<EventData> Events { get; }
    public string FlagPrefix { get; }
    public bool IsUnique { get; }
    public bool HasCleanupAttribute { get; }
    public CleanupMode CleanupMode { get; }

    public ImmutableArray<MemberData> Members { get; }
    public bool ShouldGenerateEntityComponentSources { get; }

    public bool HasEvents => Events != null && Events.Length > 0;
    
    public ComponentData(INamedTypeSymbol type)
    {
        ShortTypeName = type.Name;
        FullTypeName = type.ToCompilableString();

        ShortComponentName = ShortTypeName.RemoveComponentSuffix();
        FullComponentName = FullTypeName.RemoveDots().RemoveComponentSuffix();
        
        ComponentAttributesHelper.ParseComponentAttributes(type,
            out var contextNames,
            out var events,
            out var flagPrefix,
            out var isUnique,
            out var hasCleanupAttribute,
            out var cleanupMode);
        
        ContextNames = contextNames;
        Events = events;
        FlagPrefix = flagPrefix;
        IsUnique = isUnique;
        HasCleanupAttribute = hasCleanupAttribute;
        CleanupMode = cleanupMode;
        
        Members = ComponentMembersHelper.GetComponentMembers(type);
        ShouldGenerateEntityComponentSources = true;
    }
    
    public ComponentData(
        string shortTypeName,
        string fullTypeName,
        ImmutableArray<string> contextNames = default,
        ImmutableArray<MemberData> members = default,
        ImmutableArray<EventData> events = default,
        string flagPrefix = "is",
        bool isUnique = false,
        bool hasCleanupAttribute = false,
        CleanupMode cleanupMode = CleanupMode.DestroyEntity,
        bool shouldGenerateEntityComponentSources = false)
    {
        ShortTypeName = shortTypeName;
        FullTypeName = fullTypeName;
        
        ShortComponentName = ShortTypeName.RemoveComponentSuffix();
        FullComponentName = FullTypeName.RemoveDots().RemoveComponentSuffix();
        
        ContextNames = contextNames;
        Events = events;
        FlagPrefix = flagPrefix;
        IsUnique = isUnique;
        HasCleanupAttribute = hasCleanupAttribute;
        CleanupMode = cleanupMode;
        
        Members = members;
        
        ShouldGenerateEntityComponentSources = shouldGenerateEntityComponentSources;
    }

    public string GetComponentName() => GetComponentName(ComponentGenerationHelper.IgnoreNamespaces);
    public string GetComponentNameLowerFirst() => GetComponentName().ToLowerFirst();
    
    public string GetComponentName(bool ignoreNamespaces) 
        => ignoreNamespaces ? ShortComponentName : FullComponentName;
    
    public string GetComponentIndex(ContextData contextData)
        => $"{contextData.ContextName}{ComponentGenerationHelper.ComponentsLookupName}.{GetComponentName()}";
    
    public bool Equals(ComponentData other)
    {
        return string.Equals(FullTypeName, other.FullTypeName, StringComparison.Ordinal) &&
               string.Equals(ShortTypeName, other.ShortTypeName, StringComparison.Ordinal) &&
               ContextNames.SequenceEqual(other.ContextNames) &&
               Events.SequenceEqual(other.Events) &&
               string.Equals(FlagPrefix, other.FlagPrefix, StringComparison.Ordinal) &&
               IsUnique == other.IsUnique &&
               HasCleanupAttribute == other.HasCleanupAttribute &&
               CleanupMode == other.CleanupMode &&
               Members.SequenceEqual(other.Members) &&
               ShouldGenerateEntityComponentSources == other.ShouldGenerateEntityComponentSources;
    }

    public override bool Equals(object? obj) =>
        obj is ComponentData other && Equals(other);

    public override int GetHashCode()
    {
        unchecked // Allow arithmetic overflow (doesn't throw)
        {
            int hash = 17;
            hash = hash * 31 + (ShortTypeName?.GetHashCode() ?? 0);
            hash = hash * 31 + (FullTypeName?.GetHashCode() ?? 0);
            hash = hash * 31 + ContextNames.GetSequenceHashCode();
            hash = hash * 31 + Events.GetSequenceHashCode();
            hash = hash * 31 + (FlagPrefix?.GetHashCode() ?? 0);
            hash = hash * 31 + IsUnique.GetHashCode();
            hash = hash * 31 + HasCleanupAttribute.GetHashCode();
            hash = hash * 31 + CleanupMode.GetHashCode();
            hash = hash * 31 + Members.GetSequenceHashCode();
            hash = hash * 31 + ShouldGenerateEntityComponentSources.GetHashCode();
            return hash;
        }        
    }
    
    public static bool operator ==(ComponentData left, ComponentData right) => left.Equals(right);
    public static bool operator !=(ComponentData left, ComponentData right) => !left.Equals(right);
}