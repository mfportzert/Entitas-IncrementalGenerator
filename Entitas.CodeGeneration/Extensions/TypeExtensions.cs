using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Entitas.CodeGeneration.Extensions;

public static class TypeExtensions
{
    public static string ShortTypeName(this string fullTypeName)
    {
        var index = fullTypeName.LastIndexOf(".", StringComparison.Ordinal) + 1;
        return fullTypeName.Substring(index, fullTypeName.Length - index);
    }
    
    public static string RemoveDots(this string fullTypeName) =>
        fullTypeName.Replace(".", string.Empty);
    
    readonly static SymbolDisplayFormat CodeGenDisplayFormat = new (
        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
        miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes
    );
    
    public static string ToCompilableString(this ITypeSymbol type)
    {
        return type.ToDisplayString(CodeGenDisplayFormat);
    }
    
    public static ITypeSymbol GetPublicMemberType(this ISymbol member) 
        => (member is IFieldSymbol symbol) ? symbol.Type : ((IPropertySymbol)member).Type;
    
    public static ImmutableArray<ISymbol> GetPublicMembers(this ITypeSymbol type, bool includeBaseTypeMembers)
    {
        var membersBuilder = ImmutableArray.CreateBuilder<ISymbol>();
        
        void CollectMembers(ITypeSymbol? current)
        {
            if (current == null || current.SpecialType == SpecialType.System_Object)
                return;

            foreach (var member in current.GetMembers())
            {
                if (IsPublicMember(member))
                    membersBuilder.Add(member);
            }

            if (includeBaseTypeMembers)
                CollectMembers(current.BaseType);
        }
        
        CollectMembers(type);
        return membersBuilder.ToImmutable();
    }
    
    public static bool IsPublicMember(this ISymbol symbol) =>
        symbol.DeclaredAccessibility == Accessibility.Public
        && !symbol.IsStatic
        && symbol.CanBeReferencedByName
        && (symbol is IFieldSymbol || IsAutoProperty(symbol));
    
    public static bool IsAutoProperty(this ISymbol symbol)
    {
        if (symbol is not IPropertySymbol property)
            return false;

        // Must have both getter and setter
        if (property.GetMethod == null || property.SetMethod == null)
            return false;

        // Check if both accessors are compiler-generated (auto-properties)
        return IsAutoAccessor(property.GetMethod) && IsAutoAccessor(property.SetMethod);
    }

    public static bool IsAutoAccessor(this IMethodSymbol method)
    {
        // Accessors should have no user-defined body
        var syntaxRef = method.DeclaringSyntaxReferences.FirstOrDefault();
        if (syntaxRef == null)
            return true; // From metadata — probably auto

        var syntax = syntaxRef.GetSyntax();
        return syntax switch
        {
            AccessorDeclarationSyntax accessor => accessor.Body == null && accessor.ExpressionBody == null,
            _ => true // fallback, assume auto
        };
    }
}