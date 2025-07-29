using Entitas.CodeGeneration.Attributes;
using Entitas.CodeGeneration.Components.Extensions;
using Entitas.CodeGeneration.Contexts.Data;
using Entitas.CodeGeneration.Events.Extensions;

namespace Entitas.CodeGeneration.Components.Data;

public readonly struct EventData : IEquatable<EventData>
{
    public EventTarget EventTarget { get; }
    public EventType EventType { get; }
    public int Priority { get; }

    public EventData(EventTarget eventTarget, EventType eventType, int priority)
    {
        EventTarget = eventTarget;
        EventType = eventType;
        Priority = priority;
    }
    
    public bool Equals(EventData other)
    {
        return EventTarget == other.EventTarget &&
               EventType == other.EventType &&
               Priority == other.Priority;
    }

    public override bool Equals(object? obj) =>
        obj is EventData other && Equals(other);

    public override int GetHashCode()
    {
        unchecked // Allow arithmetic overflow (doesn't throw)
        {
            int hash = 17;
            hash = hash * 31 + (int)EventTarget;
            hash = hash * 31 + (int)EventType;
            hash = hash * 31 + Priority;
            return hash;
        }
    }
    
    public static bool operator ==(EventData left, EventData right) => left.Equals(right);
    public static bool operator !=(EventData left, EventData right) => !left.Equals(right);
}