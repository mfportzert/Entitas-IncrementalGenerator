using System.Collections.Immutable;
using Entitas.CodeGeneration.Cleanup;
using Entitas.CodeGeneration.Components;
using Entitas.CodeGeneration.Components.Data;
using Entitas.CodeGeneration.ComponentsLookups;
using Entitas.CodeGeneration.Contexts;
using Entitas.CodeGeneration.Contexts.Data;
using Entitas.CodeGeneration.EntityIndex;
using Entitas.CodeGeneration.Events;
using Entitas.CodeGeneration.VisualDebugging;
using Microsoft.CodeAnalysis;

namespace Entitas.CodeGeneration;

[Generator]
public class EntitasGenerator : IIncrementalGenerator
{
    const string MainAssembly = "Assembly-CSharp"; // TODO: Add config (in .editorconfig?)
    const string TestsAssembly = "Entitas.CodeGeneration-Tests";
    
    const bool VisualDebuggingGenerationEnabled = true; // TODO: Add config (in .editorconfig?)
    
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var shouldRun = context.CompilationProvider.Select(static (compilation, _) 
            => compilation.AssemblyName is MainAssembly or TestsAssembly);
    
        var contextsData = ContextGenerationHelper.GetContextsData(context);
        RegisterContextsGeneration(context, shouldRun, contextsData);
        
        var componentsData = ComponentGenerationHelper.GetComponentsData(context);
        var componentsByContextNameLookup = ComponentsLookupGenerationHelper.GetComponentsByContextNameLookup(componentsData);
        RegisterIndividualComponentsGeneration(context, shouldRun, contextsData, componentsByContextNameLookup);
        RegisterSharedSourcesGeneration(context, shouldRun, contextsData, componentsData, componentsByContextNameLookup);

        if (VisualDebuggingGenerationEnabled)
            RegisterVisualDebuggingGeneration(context, contextsData);
    }

    void RegisterContextsGeneration(
        IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<bool> shouldRun,
        in IncrementalValueProvider<ImmutableArray<ContextData>> contextsData)
    {
        var combinedInput = shouldRun.Combine(contextsData);
        
        // This will be triggered for any context change (ContextAttribute class)
        context.RegisterSourceOutput(combinedInput,
            static (spc, source) =>
            {
                if (!source.Left) // shouldRun
                    return;
                
                GenerateContexts(source.Right, spc);
            });
    }

    static void GenerateContexts(
        ImmutableArray<ContextData> contextsData,
        SourceProductionContext spc)
    {
        ContextGenerationHelper.GenerateContexts(spc, contextsData);
    }

    void RegisterSharedSourcesGeneration(
        IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<bool> shouldRun,
        in IncrementalValueProvider<ImmutableArray<ContextData>> contextsData,
        in IncrementalValueProvider<ImmutableArray<ComponentData>> componentsData,
        in IncrementalValueProvider<ImmutableDictionary<string, ImmutableArray<ComponentData>>> componentsByContextNameLookup)
    {
        var contextsAndComponents = contextsData.Combine(componentsData);
        var contextsAndComponentsWithLookup = contextsAndComponents.Combine(componentsByContextNameLookup);
        var combinedInput = shouldRun.Combine(contextsAndComponentsWithLookup);

        // Warning: This will be triggered for ANY context or component change
        // Keep it as light as possible
        context.RegisterSourceOutput(combinedInput,
            static (spc, source) =>
            {
                if (!source.Left) // shouldRun
                    return;

                GenerateSharedSources(source.Right, spc);
            });
    }

    static void GenerateSharedSources(
        ((ImmutableArray<ContextData>, ImmutableArray<ComponentData>), ImmutableDictionary<string, ImmutableArray<ComponentData>>) input, 
        SourceProductionContext spc)
    {
        var contextsData = input.Item1.Item1;
        var contextLookup = contextsData.ToDictionary(ctx => ctx.ContextName);

        var componentsData = input.Item1.Item2;
        ComponentGenerationHelper.TryGenerateEntityComponentInterfaces(spc, componentsData, contextLookup);

        var componentsByContextNameLookup = input.Item2;
        ComponentsLookupGenerationHelper.GenerateComponentsLookups(spc, componentsByContextNameLookup, contextLookup);
        
        EntityIndexGenerationHelper.GenerateEntityIndices(spc, componentsByContextNameLookup, contextLookup);        
        CleanupGenerationHelper.GenerateCleanupSystems(spc, componentsByContextNameLookup, contextLookup);        
        EventsGenerationHelper.GenerateEventSystems(spc, componentsByContextNameLookup, contextLookup);
    }

    void RegisterIndividualComponentsGeneration(
        IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<bool> shouldRun,
        in IncrementalValueProvider<ImmutableArray<ContextData>> contextsData,
        in IncrementalValueProvider<ImmutableDictionary<string, ImmutableArray<ComponentData>>> componentsByContextNameLookup)
    {
        // Extract all (ContextData, ComponentData) pairs
        var componentByContext = contextsData
            .Combine(componentsByContextNameLookup)
            .SelectMany((pair, _) =>
            {
                var (contexts, componentLookup) = pair;
                return contexts.SelectMany((contextData, _) =>
                {
                    return !componentLookup.TryGetValue(contextData.ContextName, out var matchingComponents) 
                        ? Enumerable.Empty<(ContextData, ComponentData)>() 
                        : matchingComponents.Select(component => (contextData, component));
                });
            });
        
        var conditionalComponentByContext = componentByContext
            .Combine(shouldRun)
            .Where(static pair => pair.Right) // filter out everything if !shouldRun
            .Select(static (pair, _) => pair.Left);
        
        // This is triggered for:
        // - individual components that have changed, or
        // - all components belonging to a context that has changed.
        context.RegisterSourceOutput(conditionalComponentByContext, 
            static (spc, source) => GenerateIndividualEntityComponent(source, spc));
    }
    
    static void GenerateIndividualEntityComponent(
        (ContextData, ComponentData) input,
        SourceProductionContext spc)
    {
        var componentData = input.Item2;
        var contextData = input.Item1;

        ComponentGenerationHelper.GenerateEntityComponent(spc, componentData, contextData);
        
        if (componentData.IsGenerated)
            return;
        
        EventsGenerationHelper.GenerateComponentEvents(spc, componentData, contextData);
        
        if (componentData.HasCleanupAttribute)
            CleanupGenerationHelper.GenerateComponentCleanupSystem(spc, componentData, contextData);
    }

    void RegisterVisualDebuggingGeneration(
        IncrementalGeneratorInitializationContext context,
        in IncrementalValueProvider<ImmutableArray<ContextData>> contextsData)
    {
        // skip for tests, to avoid bloating snapshots
        var shouldRun = context.CompilationProvider.Select(static (compilation, _) 
            => compilation.AssemblyName is MainAssembly /*or TestsAssembly*/);

        var combinedInput = shouldRun.Combine(contextsData);
        context.RegisterSourceOutput(combinedInput, 
            static (spc, source) => GenerateVisualDebugging(source, spc));
    }

    static void GenerateVisualDebugging(
        (bool, ImmutableArray<ContextData>) input,
        SourceProductionContext spc)
    {
        if (!input.Item1) // check shouldRun
            return;

        var contextsData = input.Item2;
        
        // Context Observers, Feature
        VisualDebuggingGenerationHelper.Generate(spc, contextsData);
    }
}