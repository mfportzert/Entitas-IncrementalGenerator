using System.Collections.Immutable;
using System.Text;
using Entitas.CodeGeneration.Components.Data;
using Entitas.CodeGeneration.Contexts.Data;
using Entitas.CodeGeneration.EntityIndex.Extensions;
using Entitas.CodeGeneration.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Entitas.CodeGeneration.EntityIndex;

public static class EntityIndexGenerationHelper
{
    public const string PrimaryEntityIndexTypeName = "Entitas.PrimaryEntityIndex";
    public const string EntityIndexTypeName = "Entitas.EntityIndex";

    const string? EntityIndexAttributeName = "EntityIndexAttribute";
    const string? PrimaryEntityIndexAttributeName = "PrimaryEntityIndexAttribute";

    public static bool TryFindEntityIndexType(ISymbol symbol, out EntityIndexType entityIndexType)
    {
        foreach (var attr in symbol.GetAttributes())
        {
            switch (attr.AttributeClass?.Name)
            {
                case PrimaryEntityIndexAttributeName:
                    entityIndexType = EntityIndexType.PrimaryEntityIndex;
                    return true;

                case EntityIndexAttributeName:
                    entityIndexType = EntityIndexType.EntityIndex;
                    return true;
            }
        }
        
        entityIndexType = default;
        return false;
    }

    public static void GenerateEntityIndices(SourceProductionContext spc,
        ImmutableDictionary<string, ImmutableArray<ComponentData>> componentsByContextNameLookup,
        Dictionary<string, ContextData> contextLookup)
    {
        foreach (var contextComponentsPair in componentsByContextNameLookup)
        {
            var contextName = contextComponentsPair.Key;
            if (!contextLookup.TryGetValue(contextName, out var contextData))
                continue;
            
            var componentArray = contextComponentsPair.Value;
            GenerateEntityIndices(spc, contextData, componentArray);
        }
    }

    public static void GenerateEntityIndices(SourceProductionContext spc,
        in ContextData contextData,
        in ImmutableArray<ComponentData> componentsData)
    {
        var indexConstantsBuilder = new StringBuilder();
        var addIndicesBuilder = new StringBuilder();
        var getIndicesBuilder = new StringBuilder();
        
        foreach (var componentData in componentsData)
        {
            var entityIndexCount = componentData.GetEntityIndexCount();
            if (entityIndexCount == 0)
                continue;
            
            var hasMultipleIndices = entityIndexCount > 1;

            foreach (var memberData in componentData.Members)
            {
                if (!memberData.IsEntityIndex)
                    continue;
            
                var indexName = hasMultipleIndices ?
                    componentData.FullComponentName + memberData.Name.ToUpperFirst() :
                    componentData.FullComponentName ;

                indexConstantsBuilder.AppendLine(
                    EntityIndexTemplates.IndexConstantTemplate
                        .Replace("${IndexName}", indexName));

                addIndicesBuilder.Append(
                    EntityIndexTemplates.GetAddIndexSource(
                        indexName, contextData, componentData, memberData) + "\n\n");

                var getIndexSource = memberData.EntityIndexType switch
                {
                    EntityIndexType.PrimaryEntityIndex => EntityIndexTemplates.GetPrimaryIndexSource(indexName, contextData, memberData),
                    EntityIndexType.EntityIndex => EntityIndexTemplates.GetIndexSource(indexName, contextData, memberData),
                    _ => string.Empty,
                };
                getIndicesBuilder.Append(getIndexSource+"\n\n");
            }

            var source = EntityIndexTemplates.EntityIndexContextsTemplate
                    .Replace("${indexConstants}", indexConstantsBuilder.ToString().RemoveLast("\n"))
                    .Replace("${addIndices}", addIndicesBuilder.ToString().RemoveLast("\n\n"))
                    .Replace("${getIndices}", getIndicesBuilder.ToString().RemoveLast("\n\n"));
            
            spc.AddSource(contextData.ContextName + "EntityIndices.g.cs", SourceText.From(source, Encoding.UTF8));
        }
    }
}