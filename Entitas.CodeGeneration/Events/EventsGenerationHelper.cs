using System.Collections.Immutable;
using System.Text;
using Entitas.CodeGeneration.Attributes;
using Entitas.CodeGeneration.Components.Data;
using Entitas.CodeGeneration.Components.Extensions;
using Entitas.CodeGeneration.Contexts.Data;
using Entitas.CodeGeneration.Events.Extensions;
using Entitas.CodeGeneration.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Entitas.CodeGeneration.Events;

public static class EventsGenerationHelper
{
    public static void CreateEventComponents(
        in ComponentData componentData,
        ImmutableArray<ComponentData>.Builder eventComponentsBuilder)
    {
        if (!componentData.HasEvents)
            return;
    
        var events = componentData.Events;
        var contextNames = componentData.ContextNames;
        foreach (var contextName in contextNames)
        {
            foreach (var eventData in events)
            {
                var optionalContextName = componentData.ContextNames.Length > 1 ? contextName : string.Empty;
                var eventTypeSuffix = eventData.GetEventTypeSuffix();
                var listenerComponentName = componentData.EventComponentName(eventData) + eventTypeSuffix.AddListenerSuffix();
                var listenerComponentTypeName = listenerComponentName.AddComponentSuffix();
                var listenerComponentFullTypeName = optionalContextName + listenerComponentTypeName;
    
                var eventComponentData = new ComponentData(
                    shortTypeName: listenerComponentTypeName,
                    fullTypeName: listenerComponentFullTypeName,
                    contextNames: ImmutableArray.Create(contextName),
                    members: ImmutableArray.Create(
                        new MemberData($"System.Collections.Generic.List<I{listenerComponentName}>", "value")
                    ),
                    shouldGenerateEntityComponentSources: false // event listener components have custom sources
                );
                
                eventComponentsBuilder.Add(eventComponentData);
            }
        }
    }
    
    public static void GenerateComponentEvents(SourceProductionContext spc, 
        in ComponentData componentData,
        in ContextData contextData)
    {
        foreach (var eventData in componentData.Events)
        {
            var eventListener = componentData.EventListener(contextData.ContextName, eventData);
            var eventListenerComponent = eventListener.AddComponentSuffix();

            GenerateEventListenerComponent(spc, eventListener, eventListenerComponent);
            GenerateEventEntityApi(spc, contextData, eventListener, eventListenerComponent);
            GenerateEventListenerInterface(spc, componentData, eventData, contextData, eventListener);
            GenerateEventSystem(spc, componentData, eventData, contextData, eventListener);
        }
    }

    static void GenerateEventListenerComponent(SourceProductionContext spc,
        string eventListener,
        string eventListenerComponent)
    {
        var eventListenerComponentSource = EventsTemplates.EventListenerComponentTemplate
            .Replace("${EventListenerComponent}", eventListenerComponent)
            .Replace("${EventListener}", eventListener);
            
        spc.AddSource($"{eventListenerComponent}.g.cs", SourceText.From(eventListenerComponentSource, Encoding.UTF8));
    }

    static void GenerateEventEntityApi(SourceProductionContext spc,
        in ContextData contextData,
        string eventListener,
        string eventListenerComponent)
    {
        var eventListenerLower = eventListener.ToLowerFirst();

        var eventEntitySource = EventsTemplates.EventEntityApiTemplate
            .Replace("${EntityType}", contextData.EntityTypeName)
            .Replace("${EventListener}", eventListener)
            .Replace("${eventListener}", eventListenerLower);

        spc.AddSource($"{contextData.ContextName + eventListenerComponent}.g.cs", 
            SourceText.From(eventEntitySource, Encoding.UTF8));
    }
    
    static void GenerateEventListenerInterface(SourceProductionContext spc,
        in ComponentData componentData,
        in EventData eventData,
        in ContextData contextData,
        string eventListener)
    {
        var contextName = contextData.ContextName;
        
        var members = componentData.Members;
        if (members.Length == 0)
        {
            members = ImmutableArray.Create(
                new MemberData("bool", componentData.PrefixedComponentName()));
        }
        
        var methodParameters = componentData.GetEventMethodArgs(eventData, $", {members.GetMethodParameters(false)}");
        
        var eventEntitySource = EventsTemplates.EventListenerInterfaceTemplate
            .Replace("${EventListener}", eventListener)
            .Replace("${EventComponentName}", componentData.EventComponentName(eventData))
            .Replace("${EventType}", eventData.GetEventTypeSuffix())
            .Replace("${ContextName}", contextName)
            .Replace("${methodParameters}", methodParameters);

        spc.AddSource($"I{eventListener}.g.cs", SourceText.From(eventEntitySource, Encoding.UTF8));
    }

    static void GenerateEventSystem(SourceProductionContext spc,
        in ComponentData componentData,
        in EventData eventData,
        in ContextData contextData,
        string eventListener)
    {
        var eventListenerLower = eventListener.ToLowerFirst();

        var members = componentData.Members;
        var methodArgs = componentData.GetEventMethodArgs(eventData, ", " + (members.Length == 0
            ? componentData.PrefixedComponentName()
            : string.Join(", ", members.Select(memberData => $"component.{memberData.Name}"))));

        var cachedAccess = componentData.Members.Length == 0
            ? string.Empty
            : $"var component = e.{componentData.ComponentNameValidLowerFirst()};";

        if (eventData.EventType == EventType.Removed)
        {
            methodArgs = string.Empty;
            cachedAccess = string.Empty;
        }

        var template = eventData.EventTarget == EventTarget.Self
            ? EventsTemplates.SelfTargetEventSystemTemplate
            : EventsTemplates.AnyTargetEventSystemTemplate;

        var eventName = componentData.EventName(contextData.ContextName, eventData);
        var contextNameLower = contextData.ContextName.ToLowerFirst();
        
        var source = template
            .Replace("${GroupEvent}", eventData.EventType.ToString())
            .Replace("${filter}", GetFilter(componentData, contextData.ContextName, eventData))
            .Replace("${cachedAccess}", cachedAccess)
            .Replace("${methodArgs}", methodArgs)
            .Replace("${Event}", eventName)
            .Replace("${EventType}", eventData.GetEventTypeSuffix())
            .Replace("${EntityType}", contextData.EntityTypeName)
            .Replace("${EventListener}", eventListener)
            .Replace("${eventListener}", eventListenerLower)
            .Replace("${contextName}", contextNameLower)
            .Replace("${MatcherType}", contextData.MatcherTypeName)
            .Replace("${ComponentName}", componentData.GetComponentName())
            .Replace("${EventComponentName}", componentData.EventComponentName(eventData));
            
        spc.AddSource(eventName+"EventSystem.g.cs", SourceText.From(source, Encoding.UTF8));
    }
    
    static string GetFilter(in ComponentData componentData, 
        string contextName, 
        in EventData eventData)
    {
        var filter = string.Empty;
        if (componentData.Members.Length == 0)
        {
            switch (eventData.EventType)
            {
                case EventType.Added:
                    filter = $"entity.{componentData.PrefixedComponentName()}";
                    break;
                case EventType.Removed:
                    filter = $"!entity.{componentData.PrefixedComponentName()}";
                    break;
            }
        }
        else
        {
            switch (eventData.EventType)
            {
                case EventType.Added:
                    filter = $"entity.has{componentData.GetComponentName()}";
                    break;
                case EventType.Removed:
                    filter = $"!entity.has{componentData.GetComponentName()}";
                    break;
            }
        }

        if (eventData.EventTarget == EventTarget.Self)
            filter += $" && entity.has{componentData.EventListener(contextName, eventData)}";

        return filter;
    }

    public static void GenerateEventSystems(SourceProductionContext spc, 
        ImmutableDictionary<string, ImmutableArray<ComponentData>> componentsByContextNameLookup,
        Dictionary<string, ContextData> contextLookup)
    {
        var contextNameToDataTuple = new Dictionary<string, IEnumerable<(ComponentData, EventData)>>();
        foreach (var kvp in componentsByContextNameLookup)
        {
            var orderedEventData = kvp.Value
                .Where(c => c.HasEvents)
                .SelectMany(c => c.Events.Select(eventData => (component: c, eventData)))
                .OrderBy(tuple => tuple.eventData.Priority)
                .ThenBy(tuple => tuple.component.GetComponentName());

            contextNameToDataTuple[kvp.Key] = orderedEventData;
        }
        
        foreach (var kvp in contextNameToDataTuple)
        {
            var contextName = kvp.Key;
            if (!contextLookup.TryGetValue(contextName, out var contextData))
                continue;

            var systemsList = GenerateSystemList(contextName, kvp.Value);
            var source = EventsTemplates.EventSystemsTemplate
                .Replace("${ContextName}", contextData.ContextName)
                .Replace("${systemsList}", systemsList);
            
            spc.AddSource(contextData.ContextName + "EventSystems.g.cs", SourceText.From(source, Encoding.UTF8));
        }
    }
    
    static string GenerateSystemList(string contextName, IEnumerable<(ComponentData, EventData)> data) =>
        string.Join("\n", data.SelectMany(tuple => GenerateSystemListForData(contextName, tuple)));

    static IEnumerable<string> GenerateSystemListForData(string contextName, (ComponentData component, EventData eventData) data)
    {
        return data.component.ContextNames
            .Where(ctxName => ctxName == contextName)
            .Select(ctxName => GenerateAddSystem(ctxName, data));
    }

    static string GenerateAddSystem(string contextName, (ComponentData component, EventData eventData) data)
    {
        return EventsTemplates.EventSystemAddTemplate
            .Replace("${priority}", data.eventData.Priority.ToString())
            .Replace("${Event}", data.component.EventName(contextName, data.eventData));
    }
}