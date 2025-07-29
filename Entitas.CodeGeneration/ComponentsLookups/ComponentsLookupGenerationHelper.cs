using System.Collections.Immutable;
using System.Text;
using Entitas.CodeGeneration.Components;
using Entitas.CodeGeneration.Components.Data;
using Entitas.CodeGeneration.Contexts.Data;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Entitas.CodeGeneration.ComponentsLookups;

public static class ComponentsLookupGenerationHelper
{
    public static IncrementalValueProvider<ImmutableDictionary<string, ImmutableArray<ComponentData>>> 
        GetComponentsByContextNameLookup(IncrementalValueProvider<ImmutableArray<ComponentData>> componentsData)
    {
        return componentsData
            .Select((componentDataArray, _) =>
            {
                var tempBuilders = new Dictionary<string, ImmutableArray<ComponentData>.Builder>();
                
                // we could order the components, but generated files are mostly invisible to devs, and we rarely check ComponentLookups
                //var orderedComponents = componentDataArray.OrderBy(static c => c.GetComponentName());
                
                foreach (var component in componentDataArray)
                {
                    foreach (var ctx in component.ContextNames)
                    {
                        if (!tempBuilders.TryGetValue(ctx, out var builder))
                        {
                            builder = ImmutableArray.CreateBuilder<ComponentData>();
                            tempBuilders[ctx] = builder;
                        }
                        builder.Add(component);
                    }
                }

                var finalBuilder = ImmutableDictionary.CreateBuilder<string, ImmutableArray<ComponentData>>();
                foreach (var kvp in tempBuilders)
                    finalBuilder[kvp.Key] = kvp.Value.ToImmutable();

                return finalBuilder.ToImmutable();
            });
    }
    
    public static void GenerateComponentsLookups(SourceProductionContext spc,
        ImmutableDictionary<string, ImmutableArray<ComponentData>> componentsByContextNameLookup,
        Dictionary<string, ContextData> contextLookup)
    {
        foreach (var contextComponentsPair in componentsByContextNameLookup)
        {
            var contextName = contextComponentsPair.Key;
            if (!contextLookup.TryGetValue(contextName, out var contextData))
                continue;

            var componentArray = contextComponentsPair.Value;
            GenerateComponentsLookup(spc, contextData, componentArray);
        }
        
        // Generate empty lookups (when there are no components in context)
        foreach (var contextEntry in contextLookup)
        {
            var contextName = contextEntry.Key;
            if (!componentsByContextNameLookup.TryGetValue(contextName, out var components)
                || componentsByContextNameLookup[contextName].IsDefaultOrEmpty)
            {
                var emptyComponentsArray = ImmutableArray<ComponentData>.Empty;
                GenerateComponentsLookup(spc, contextEntry.Value, emptyComponentsArray);
            }
        }
    }

    public static void GenerateComponentsLookup(SourceProductionContext spc,
        in ContextData contextData,
        in ImmutableArray<ComponentData> componentsData)
    {
        var componentConstantsList = string.Join("\n", componentsData
            .Select((c, index) => ComponentsLookupTemplates.ComponentConstantTemplate
                .Replace("${ComponentName}", c.GetComponentName())
                .Replace("${Index}", index.ToString())));

        var totalComponentsConstant = ComponentsLookupTemplates.TotalComponentsConstantTemplate
            .Replace("${totalComponents}", componentsData.Length.ToString());
        
        var componentNamesList = string.Join(",\n", componentsData
            .Select(c => ComponentsLookupTemplates.ComponentNameTemplate
                .Replace("${ComponentName}", c.GetComponentName())));
        
        var componentTypesList = string.Join(",\n", componentsData
            .Select(c => ComponentsLookupTemplates.ComponentTypeTemplate
                .Replace("${ComponentType}", c.FullTypeName)));
        
        var lookupClassName = contextData.ContextName + ComponentGenerationHelper.ComponentsLookupName;
        var source = ComponentsLookupTemplates.ComponentsLookupTemplate
            .Replace("${Lookup}", lookupClassName)
            .Replace("${componentConstantsList}", componentConstantsList)
            .Replace("${totalComponentsConstant}", totalComponentsConstant)
            .Replace("${componentNamesList}", componentNamesList)
            .Replace("${componentTypesList}", componentTypesList);
            
        spc.AddSource(contextData.ComponentsLookupTypeName + ".g.cs", SourceText.From(source, Encoding.UTF8));
    }
}