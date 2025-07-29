using Entitas.CodeGeneration.Attributes;
using Entitas.CodeGeneration.Components.Data;
using Entitas.CodeGeneration.Components.Extensions;

namespace Entitas.CodeGeneration.Events.Extensions;

public static class ComponentDataExtensions
{
    // ex: AnyPosition3
    public static string EventComponentName(this ComponentData componentData, EventData eventData)
    {
        var componentName = componentData.GetComponentName();
        var shortComponentName = componentData.ShortComponentName;
        var eventComponentName = componentName.Replace(
            shortComponentName,
            eventData.GetEventPrefix() + shortComponentName
        );
        return eventComponentName;
    }
    
    // ex: (Context)AnyPosition3Removed
    public static string EventName(this ComponentData componentData, string contextName, EventData eventData)
    {
        var optionalContextName = componentData.ContextNames.Length > 1 ? contextName : string.Empty;
        return optionalContextName + EventComponentName(componentData, eventData) + eventData.GetEventTypeSuffix();
    }

    // ex: (Context)AnyPosition3RemovedListener
    public static string EventListener(this ComponentData componentData, string contextName, EventData eventData) =>
        componentData.EventName(contextName, eventData).AddListenerSuffix();
    
    public static string GetEventMethodArgs(this ComponentData componentData, EventData eventData, string args)
    {
        if (componentData.Members.Length == 0)
            return string.Empty;

        return eventData.EventType == EventType.Removed ? string.Empty : args;
    }
}