namespace Entitas.CodeGeneration.Events;

public static class EventsTemplates
{
    public const string EventEntityApiTemplate =
        @"public partial class ${EntityType}
{
    public void Add${EventListener}(I${EventListener} value)
    {
        var listeners = has${EventListener}
            ? ${eventListener}.value
            : new System.Collections.Generic.List<I${EventListener}>();
        listeners.Add(value);
        Replace${EventListener}(listeners);
    }

    public void Remove${EventListener}(I${EventListener} value, bool removeComponentWhenEmpty = true)
    {
        var listeners = ${eventListener}.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0)
        {
            Remove${EventListener}();
        }
        else
        {
            Replace${EventListener}(listeners);
        }
    }
}
";
    
    public const string EventListenerComponentTemplate =
        @"[Entitas.CodeGeneration.Attributes.DontGenerate(false)]
public sealed class ${EventListenerComponent} : Entitas.IComponent
{
    public System.Collections.Generic.List<I${EventListener}> value;
}
";
    
    public const string EventListenerInterfaceTemplate =
        @"public interface I${EventListener}
{
    void On${EventComponentName}${EventType}(${ContextName}Entity entity${methodParameters});
}
";
    
    public const string AnyTargetEventSystemTemplate =
            @"public sealed class ${Event}EventSystem : Entitas.ReactiveSystem<${EntityType}>
{
    readonly Entitas.IGroup<${EntityType}> _listeners;
    readonly System.Collections.Generic.List<${EntityType}> _entityBuffer;
    readonly System.Collections.Generic.List<I${EventListener}> _listenerBuffer;

    public ${Event}EventSystem(Contexts contexts) : base(contexts.${contextName})
    {
        _listeners = contexts.${contextName}.GetGroup(${MatcherType}.${EventListener});
        _entityBuffer = new System.Collections.Generic.List<${EntityType}>();
        _listenerBuffer = new System.Collections.Generic.List<I${EventListener}>();
    }

    protected override Entitas.ICollector<${EntityType}> GetTrigger(Entitas.IContext<${EntityType}> context)
    {
        return Entitas.CollectorContextExtension.CreateCollector(
            context, Entitas.TriggerOnEventMatcherExtension.${GroupEvent}(${MatcherType}.${ComponentName})
        );
    }

    protected override bool Filter(${EntityType} entity)
    {
        return ${filter};
    }

    protected override void Execute(System.Collections.Generic.List<${EntityType}> entities)
    {
        foreach (var e in entities)
        {
            ${cachedAccess}
            foreach (var listenerEntity in _listeners.GetEntities(_entityBuffer))
            {
                _listenerBuffer.Clear();
                _listenerBuffer.AddRange(listenerEntity.${eventListener}.value);
                foreach (var listener in _listenerBuffer)
                {
                    listener.On${EventComponentName}${EventType}(e${methodArgs});
                }
            }
        }
    }
}
";

    public const string SelfTargetEventSystemTemplate =
            @"public sealed class ${Event}EventSystem : Entitas.ReactiveSystem<${EntityType}>
{
    readonly System.Collections.Generic.List<I${EventListener}> _listenerBuffer;

    public ${Event}EventSystem(Contexts contexts) : base(contexts.${contextName})
    {
        _listenerBuffer = new System.Collections.Generic.List<I${EventListener}>();
    }

    protected override Entitas.ICollector<${EntityType}> GetTrigger(Entitas.IContext<${EntityType}> context)
    {
        return Entitas.CollectorContextExtension.CreateCollector(
            context, Entitas.TriggerOnEventMatcherExtension.${GroupEvent}(${MatcherType}.${ComponentName})
        );
    }

    protected override bool Filter(${EntityType} entity)
    {
        return ${filter};
    }

    protected override void Execute(System.Collections.Generic.List<${EntityType}> entities)
    {
        foreach (var e in entities)
        {
            ${cachedAccess}
            _listenerBuffer.Clear();
            _listenerBuffer.AddRange(e.${eventListener}.value);
            foreach (var listener in _listenerBuffer)
            {
                listener.On${ComponentName}${EventType}(e${methodArgs});
            }
        }
    }
}
";
    
    public const string EventSystemsTemplate =
        @"public sealed class ${ContextName}EventSystems : Feature
{
    public ${ContextName}EventSystems(Contexts contexts)
    {
${systemsList}
    }
}
";
    
    public const string EventSystemAddTemplate = @"        Add(new ${Event}EventSystem(contexts)); // priority: ${priority}";
}