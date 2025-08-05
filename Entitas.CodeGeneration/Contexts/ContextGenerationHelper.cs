using System.Collections.Immutable;
using System.Text;
using Entitas.CodeGeneration.Components;
using Entitas.CodeGeneration.Contexts.Data;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Entitas.CodeGeneration.Contexts;

public static class ContextGenerationHelper
{
    public const string DefaultContextName = "Game";
    
    public readonly static string? ContextAttributeName = "ContextAttribute";
    public readonly static string? ContextAttributeTypeName = "Entitas.CodeGeneration.Attributes.ContextAttribute";

    const string AttributeName = "Attribute";
    
    public static IncrementalValueProvider<ImmutableArray<ContextData>> GetContextsData(IncrementalGeneratorInitializationContext context)
    {
        var contextsData = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => IsContextCandidateSyntax(s),
                transform: static (ctx, _) => TryGetContextData(ctx))
            .Where(static c => c is not null)
            .Select(static (c, _) => (ContextData)c!) // null-forgiving operator, safe after filtering
            .Collect();
        
        return contextsData;
    }
    
    static bool IsContextCandidateSyntax(SyntaxNode node)
    {
        if (node is not ClassDeclarationSyntax c || c.BaseList == null)
            return false;

        if (!c.Identifier.Text.EndsWith(AttributeName))
            return false;
        
        foreach (var baseTypeSyntax in c.BaseList.Types)
        {
            var baseTypeName = baseTypeSyntax.Type switch
            {
                IdentifierNameSyntax id => id.Identifier.Text,
                QualifiedNameSyntax q => q.Right.Identifier.Text,
                _ => null,
            };
            
            if (baseTypeName == ContextAttributeName)
                return true;
        }
        
        return false;
    }
    
    static ContextData? TryGetContextData(GeneratorSyntaxContext context)
    {
        var classSyntax = (ClassDeclarationSyntax) context.Node;
        
        // It's way faster to parse the class name
        // than to search for the name constant in base constructor.
        // (Just need to enforce naming conventions)
        var contextName = classSyntax.Identifier.Text
            .Replace(AttributeName, string.Empty);

        return new ContextData(contextName);
        
        // if (ModelExtensions.GetDeclaredSymbol(context.SemanticModel, classSyntax) is not INamedTypeSymbol classTypeSymbol)
        //     return null;
        //
        // if (classTypeSymbol.IsAbstract)
        //     return null;
        //
        // // Contexts are found by looking for classes that inherit ContextAttribute
        // // ex: public sealed class InputAttribute : Entitas.CodeGeneration.Attributes.ContextAttribute
        // var baseTypeStr = classTypeSymbol.BaseType?.ToDisplayString();
        // if (baseTypeStr == ContextAttributeTypeName)
        // {
        //     // We'll find the name in the base constructor
        //     // ex: public InputAttribute() : base("Input")
        //     var ctor = classTypeSymbol.InstanceConstructors.FirstOrDefault();
        //     var ctorSyntax = ctor?.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as ConstructorDeclarationSyntax;
        //     var baseArg = ctorSyntax?.Initializer?.ArgumentList.Arguments.FirstOrDefault()?.Expression;
        //
        //     var value = context.SemanticModel.GetConstantValue(baseArg!);
        //     if (value.HasValue)
        //     {
        //         var contextName = value.Value as string;
        //         return new ContextData(contextName!);
        //     }
        // }
        //
        // return null;
    }
    
    public static void GenerateContexts(SourceProductionContext spc, ImmutableArray<ContextData> contextsData)
    {
        GenerateContextsSource(spc, contextsData);
        
        foreach (var contextData in contextsData)
        {
            GenerateContext(spc, contextData);
            GenerateContextMatcher(spc, contextData);
            GenerateContextEntity(spc, contextData);
        }
    }

    public static void GenerateContextsSource(SourceProductionContext spc, ImmutableArray<ContextData> contextsData)
    {
        var contextList = string.Join(", ", contextsData
            .Select(contextData => ContextTemplates.ContextListTemplate
                .Replace("${contextName}", contextData.ContextName)));

        var contextPropertyList = string.Join("\n", contextsData
            .Select(contextData => ContextTemplates.ContextPropertyTemplate
                .Replace("${contextName}", contextData.ContextName)
                .Replace("${ContextType}", contextData.ContextTypeName)));

        var contextAssignmentList = string.Join("\n", contextsData
            .Select(contextData => ContextTemplates.ContextAssignmentTemplate
                .Replace("${contextName}", contextData.ContextName)
                .Replace("${ContextType}", contextData.ContextTypeName)));

        var generatedSource = ContextTemplates.ContextsTemplate
            .Replace("${contextList}", contextList)
            .Replace("${contextPropertyList}", contextPropertyList)
            .Replace("${contextAssignmentList}", contextAssignmentList);
        
        spc.AddSource("Contexts.g.cs", SourceText.From(generatedSource, Encoding.UTF8));
    }
    
    public static void GenerateContext(SourceProductionContext spc, ContextData data)
    {
        var generatedSource = ContextTemplates.ContextTemplate
            .Replace("${ContextName}", data.ContextName)
            .Replace("${ContextType}", data.ContextTypeName)
            .Replace("${EntityType}", data.EntityTypeName)
            .Replace("${Lookup}", data.ContextName + ComponentGenerationHelper.ComponentsLookupName);
        
        spc.AddSource(data.ContextTypeName + ".g.cs", SourceText.From(generatedSource, Encoding.UTF8));
    }

    public static void GenerateContextMatcher(SourceProductionContext spc, ContextData data)
    {
        var generatedSource = ContextTemplates.ContextMatcherTemplate
            .Replace("${MatcherType}", data.MatcherTypeName)
            .Replace("${EntityType}", data.EntityTypeName);
            
        spc.AddSource(data.MatcherTypeName + ".g.cs", SourceText.From(generatedSource, Encoding.UTF8));
    }

    public static void GenerateContextEntity(SourceProductionContext spc, ContextData data)
    {
        var source = ContextTemplates.ContextEntityTemplate
            .Replace("${EntityType}", data.EntityTypeName);
        
        spc.AddSource(data.EntityTypeName + ".g.cs", SourceText.From(source, Encoding.UTF8));
    }
}