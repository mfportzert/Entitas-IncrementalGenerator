using Entitas.CodeGeneration.Components.Data;

namespace Entitas.CodeGeneration.Components.Extensions;

public static class EventDataExtensions
{
    const string RemovedEventTypeSuffix = "Removed";
    const string AnyEventTypeSuffix = "Any";
    
    public static string GetEventTypeSuffix(this EventData eventData) =>
        eventData.EventType == EventType.Removed ? RemovedEventTypeSuffix : string.Empty;

    public static string GetEventPrefix(this EventData eventData) =>
        eventData.EventTarget == EventTarget.Any ? AnyEventTypeSuffix : string.Empty;
}