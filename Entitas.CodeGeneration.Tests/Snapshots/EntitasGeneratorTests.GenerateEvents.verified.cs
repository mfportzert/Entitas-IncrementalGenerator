// AnyTestEventEventSystem.g.cs
public sealed class AnyTestEventEventSystem : Entitas.ReactiveSystem<GameEntity>
{
    readonly Entitas.IGroup<GameEntity> _listeners;
    readonly System.Collections.Generic.List<GameEntity> _entityBuffer;
    readonly System.Collections.Generic.List<IAnyTestEventListener> _listenerBuffer;

    public AnyTestEventEventSystem(Contexts contexts) : base(contexts.game)
    {
        _listeners = contexts.game.GetGroup(GameMatcher.AnyTestEventListener);
        _entityBuffer = new System.Collections.Generic.List<GameEntity>();
        _listenerBuffer = new System.Collections.Generic.List<IAnyTestEventListener>();
    }

    protected override Entitas.ICollector<GameEntity> GetTrigger(Entitas.IContext<GameEntity> context)
    {
        return Entitas.CollectorContextExtension.CreateCollector(
            context, Entitas.TriggerOnEventMatcherExtension.Added(GameMatcher.TestEvent)
        );
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasTestEvent;
    }

    protected override void Execute(System.Collections.Generic.List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var component = e.testEvent;
            foreach (var listenerEntity in _listeners.GetEntities(_entityBuffer))
            {
                _listenerBuffer.Clear();
                _listenerBuffer.AddRange(listenerEntity.anyTestEventListener.value);
                foreach (var listener in _listenerBuffer)
                {
                    listener.OnAnyTestEvent(e, component.Value);
                }
            }
        }
    }
}


// AnyTestEventListenerComponent.g.cs
[Entitas.CodeGeneration.Attributes.DontGenerate]
public sealed class AnyTestEventListenerComponent : Entitas.IComponent
{
    public System.Collections.Generic.List<IAnyTestEventListener> value;
}


// AnyTestEventRemovedEventSystem.g.cs
public sealed class AnyTestEventRemovedEventSystem : Entitas.ReactiveSystem<GameEntity>
{
    readonly Entitas.IGroup<GameEntity> _listeners;
    readonly System.Collections.Generic.List<GameEntity> _entityBuffer;
    readonly System.Collections.Generic.List<IAnyTestEventRemovedListener> _listenerBuffer;

    public AnyTestEventRemovedEventSystem(Contexts contexts) : base(contexts.game)
    {
        _listeners = contexts.game.GetGroup(GameMatcher.AnyTestEventRemovedListener);
        _entityBuffer = new System.Collections.Generic.List<GameEntity>();
        _listenerBuffer = new System.Collections.Generic.List<IAnyTestEventRemovedListener>();
    }

    protected override Entitas.ICollector<GameEntity> GetTrigger(Entitas.IContext<GameEntity> context)
    {
        return Entitas.CollectorContextExtension.CreateCollector(
            context, Entitas.TriggerOnEventMatcherExtension.Removed(GameMatcher.TestEvent)
        );
    }

    protected override bool Filter(GameEntity entity)
    {
        return !entity.hasTestEvent;
    }

    protected override void Execute(System.Collections.Generic.List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            
            foreach (var listenerEntity in _listeners.GetEntities(_entityBuffer))
            {
                _listenerBuffer.Clear();
                _listenerBuffer.AddRange(listenerEntity.anyTestEventRemovedListener.value);
                foreach (var listener in _listenerBuffer)
                {
                    listener.OnAnyTestEventRemoved(e);
                }
            }
        }
    }
}


// AnyTestEventRemovedListenerComponent.g.cs
[Entitas.CodeGeneration.Attributes.DontGenerate]
public sealed class AnyTestEventRemovedListenerComponent : Entitas.IComponent
{
    public System.Collections.Generic.List<IAnyTestEventRemovedListener> value;
}


// Contexts.g.cs
public partial class Contexts : Entitas.IContexts
{
    public static Contexts sharedInstance
    {
        get
        {
            if (_sharedInstance == null)
            {
                _sharedInstance = new Contexts();
            }

            return _sharedInstance;
        }
        set { _sharedInstance = value; }
    }

    static Contexts _sharedInstance;

    public GameContext game { get; set; }
    public InputContext input { get; set; }

    public Entitas.IContext[] allContexts { get { return new Entitas.IContext [] { game, input }; } }

    public Contexts()
    {
        game = new GameContext();
        input = new InputContext();

        var postConstructors = System.Linq.Enumerable.Where(
            GetType().GetMethods(),
            method => System.Attribute.IsDefined(method, typeof(Entitas.CodeGeneration.Attributes.PostConstructorAttribute))
        );

        foreach (var postConstructor in postConstructors)
        {
            postConstructor.Invoke(this, null);
        }
    }

    public void Reset()
    {
        var contexts = allContexts;
        for (int i = 0; i < contexts.Length; i++)
        {
            contexts[i].Reset();
        }
    }
}


// GameAnyTestEventListenerComponent.g.cs
public partial class GameEntity
{
    public AnyTestEventListenerComponent anyTestEventListener { get { return (AnyTestEventListenerComponent)GetComponent(GameComponentsLookup.AnyTestEventListener); } }
    public bool hasAnyTestEventListener { get { return HasComponent(GameComponentsLookup.AnyTestEventListener); } }

    public void AddAnyTestEventListener(System.Collections.Generic.List<IAnyTestEventListener> newValue)
    {
        var index = GameComponentsLookup.AnyTestEventListener;
        var component = (AnyTestEventListenerComponent)CreateComponent(index, typeof(AnyTestEventListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceAnyTestEventListener(System.Collections.Generic.List<IAnyTestEventListener> newValue)
    {
        var index = GameComponentsLookup.AnyTestEventListener;
        var component = (AnyTestEventListenerComponent)CreateComponent(index, typeof(AnyTestEventListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveAnyTestEventListener()
    {
        RemoveComponent(GameComponentsLookup.AnyTestEventListener);
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherAnyTestEventListener;

    public static Entitas.IMatcher<GameEntity> AnyTestEventListener
    {
        get
        {
            if (_matcherAnyTestEventListener == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.AnyTestEventListener);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherAnyTestEventListener = matcher;
            }

            return _matcherAnyTestEventListener;
        }
    }
}


// GameAnyTestEventListenerComponentEvent.g.cs
public partial class GameEntity
{
    public void AddAnyTestEventListener(IAnyTestEventListener value)
    {
        var listeners = hasAnyTestEventListener
            ? anyTestEventListener.value
            : new System.Collections.Generic.List<IAnyTestEventListener>();
        listeners.Add(value);
        ReplaceAnyTestEventListener(listeners);
    }

    public void RemoveAnyTestEventListener(IAnyTestEventListener value, bool removeComponentWhenEmpty = true)
    {
        var listeners = anyTestEventListener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0)
        {
            RemoveAnyTestEventListener();
        }
        else
        {
            ReplaceAnyTestEventListener(listeners);
        }
    }
}


// GameAnyTestEventRemovedListenerComponent.g.cs
public partial class GameEntity
{
    public AnyTestEventRemovedListenerComponent anyTestEventRemovedListener { get { return (AnyTestEventRemovedListenerComponent)GetComponent(GameComponentsLookup.AnyTestEventRemovedListener); } }
    public bool hasAnyTestEventRemovedListener { get { return HasComponent(GameComponentsLookup.AnyTestEventRemovedListener); } }

    public void AddAnyTestEventRemovedListener(System.Collections.Generic.List<IAnyTestEventRemovedListener> newValue)
    {
        var index = GameComponentsLookup.AnyTestEventRemovedListener;
        var component = (AnyTestEventRemovedListenerComponent)CreateComponent(index, typeof(AnyTestEventRemovedListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceAnyTestEventRemovedListener(System.Collections.Generic.List<IAnyTestEventRemovedListener> newValue)
    {
        var index = GameComponentsLookup.AnyTestEventRemovedListener;
        var component = (AnyTestEventRemovedListenerComponent)CreateComponent(index, typeof(AnyTestEventRemovedListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveAnyTestEventRemovedListener()
    {
        RemoveComponent(GameComponentsLookup.AnyTestEventRemovedListener);
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherAnyTestEventRemovedListener;

    public static Entitas.IMatcher<GameEntity> AnyTestEventRemovedListener
    {
        get
        {
            if (_matcherAnyTestEventRemovedListener == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.AnyTestEventRemovedListener);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherAnyTestEventRemovedListener = matcher;
            }

            return _matcherAnyTestEventRemovedListener;
        }
    }
}


// GameAnyTestEventRemovedListenerComponentEvent.g.cs
public partial class GameEntity
{
    public void AddAnyTestEventRemovedListener(IAnyTestEventRemovedListener value)
    {
        var listeners = hasAnyTestEventRemovedListener
            ? anyTestEventRemovedListener.value
            : new System.Collections.Generic.List<IAnyTestEventRemovedListener>();
        listeners.Add(value);
        ReplaceAnyTestEventRemovedListener(listeners);
    }

    public void RemoveAnyTestEventRemovedListener(IAnyTestEventRemovedListener value, bool removeComponentWhenEmpty = true)
    {
        var listeners = anyTestEventRemovedListener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0)
        {
            RemoveAnyTestEventRemovedListener();
        }
        else
        {
            ReplaceAnyTestEventRemovedListener(listeners);
        }
    }
}


// GameCleanupSystems.g.cs
public sealed class GameCleanupSystems : Feature
{
    public GameCleanupSystems(Contexts contexts)
    {

    }
}


// GameComponentsLookup.g.cs
public static class GameComponentsLookup
{
    public const int TestEvent = 0;
    public const int NamespaceTestEvent2 = 1;
    public const int NamespaceTestEvent3 = 2;
    public const int AnyTestEventListener = 3;
    public const int AnyTestEventRemovedListener = 4;
    public const int TestEventListener = 5;
    public const int TestEventRemovedListener = 6;
    public const int NamespaceAnyTestEvent2Listener = 7;
    public const int NamespaceAnyTestEvent2RemovedListener = 8;
    public const int GameNamespaceAnyTestEvent3Listener = 9;
    public const int GameNamespaceAnyTestEvent3RemovedListener = 10;

    public const int TotalComponents = 11;

    public static readonly string[] componentNames = 
    {
        "TestEvent",
        "NamespaceTestEvent2",
        "NamespaceTestEvent3",
        "AnyTestEventListener",
        "AnyTestEventRemovedListener",
        "TestEventListener",
        "TestEventRemovedListener",
        "NamespaceAnyTestEvent2Listener",
        "NamespaceAnyTestEvent2RemovedListener",
        "GameNamespaceAnyTestEvent3Listener",
        "GameNamespaceAnyTestEvent3RemovedListener"
    };

    public static readonly System.Type[] componentTypes = 
    {
        typeof(TestEventComponent),
        typeof(Namespace.TestEvent2Component),
        typeof(Namespace.TestEvent3Component),
        typeof(AnyTestEventListenerComponent),
        typeof(AnyTestEventRemovedListenerComponent),
        typeof(TestEventListenerComponent),
        typeof(TestEventRemovedListenerComponent),
        typeof(NamespaceAnyTestEvent2ListenerComponent),
        typeof(NamespaceAnyTestEvent2RemovedListenerComponent),
        typeof(GameNamespaceAnyTestEvent3ListenerComponent),
        typeof(GameNamespaceAnyTestEvent3RemovedListenerComponent)
    };
}


// GameContext.g.cs
public sealed partial class GameContext : Entitas.Context<GameEntity>
{
    public GameContext()
        : base(
            GameComponentsLookup.TotalComponents,
            0,
            new Entitas.ContextInfo(
                "Game",
                GameComponentsLookup.componentNames,
                GameComponentsLookup.componentTypes
            ),
            (entity) =>

#if (ENTITAS_FAST_AND_UNSAFE)
                new Entitas.UnsafeAERC(),
#else
                new Entitas.SafeAERC(entity),
#endif
            () => new GameEntity()
        ) 
    {
    }
}


// GameEntity.g.cs
public sealed partial class GameEntity : Entitas.Entity
{
}


// GameEventSystems.g.cs
public sealed class GameEventSystems : Feature
{
    public GameEventSystems(Contexts contexts)
    {
        Add(new NamespaceAnyTestEvent2EventSystem(contexts)); // priority: 0
        Add(new NamespaceAnyTestEvent2RemovedEventSystem(contexts)); // priority: 0
        Add(new GameNamespaceAnyTestEvent3EventSystem(contexts)); // priority: 0
        Add(new GameNamespaceAnyTestEvent3RemovedEventSystem(contexts)); // priority: 0
        Add(new AnyTestEventEventSystem(contexts)); // priority: 0
        Add(new AnyTestEventRemovedEventSystem(contexts)); // priority: 0
        Add(new TestEventEventSystem(contexts)); // priority: 0
        Add(new TestEventRemovedEventSystem(contexts)); // priority: 0
    }
}


// GameGameNamespaceAnyTestEvent3ListenerComponent.g.cs
public partial class GameEntity
{
    public GameNamespaceAnyTestEvent3ListenerComponent gameNamespaceAnyTestEvent3Listener { get { return (GameNamespaceAnyTestEvent3ListenerComponent)GetComponent(GameComponentsLookup.GameNamespaceAnyTestEvent3Listener); } }
    public bool hasGameNamespaceAnyTestEvent3Listener { get { return HasComponent(GameComponentsLookup.GameNamespaceAnyTestEvent3Listener); } }

    public void AddGameNamespaceAnyTestEvent3Listener(System.Collections.Generic.List<IGameNamespaceAnyTestEvent3Listener> newValue)
    {
        var index = GameComponentsLookup.GameNamespaceAnyTestEvent3Listener;
        var component = (GameNamespaceAnyTestEvent3ListenerComponent)CreateComponent(index, typeof(GameNamespaceAnyTestEvent3ListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceGameNamespaceAnyTestEvent3Listener(System.Collections.Generic.List<IGameNamespaceAnyTestEvent3Listener> newValue)
    {
        var index = GameComponentsLookup.GameNamespaceAnyTestEvent3Listener;
        var component = (GameNamespaceAnyTestEvent3ListenerComponent)CreateComponent(index, typeof(GameNamespaceAnyTestEvent3ListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveGameNamespaceAnyTestEvent3Listener()
    {
        RemoveComponent(GameComponentsLookup.GameNamespaceAnyTestEvent3Listener);
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherGameNamespaceAnyTestEvent3Listener;

    public static Entitas.IMatcher<GameEntity> GameNamespaceAnyTestEvent3Listener
    {
        get
        {
            if (_matcherGameNamespaceAnyTestEvent3Listener == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.GameNamespaceAnyTestEvent3Listener);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherGameNamespaceAnyTestEvent3Listener = matcher;
            }

            return _matcherGameNamespaceAnyTestEvent3Listener;
        }
    }
}


// GameGameNamespaceAnyTestEvent3ListenerComponentEvent.g.cs
public partial class GameEntity
{
    public void AddGameNamespaceAnyTestEvent3Listener(IGameNamespaceAnyTestEvent3Listener value)
    {
        var listeners = hasGameNamespaceAnyTestEvent3Listener
            ? gameNamespaceAnyTestEvent3Listener.value
            : new System.Collections.Generic.List<IGameNamespaceAnyTestEvent3Listener>();
        listeners.Add(value);
        ReplaceGameNamespaceAnyTestEvent3Listener(listeners);
    }

    public void RemoveGameNamespaceAnyTestEvent3Listener(IGameNamespaceAnyTestEvent3Listener value, bool removeComponentWhenEmpty = true)
    {
        var listeners = gameNamespaceAnyTestEvent3Listener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0)
        {
            RemoveGameNamespaceAnyTestEvent3Listener();
        }
        else
        {
            ReplaceGameNamespaceAnyTestEvent3Listener(listeners);
        }
    }
}


// GameGameNamespaceAnyTestEvent3RemovedListenerComponent.g.cs
public partial class GameEntity
{
    public GameNamespaceAnyTestEvent3RemovedListenerComponent gameNamespaceAnyTestEvent3RemovedListener { get { return (GameNamespaceAnyTestEvent3RemovedListenerComponent)GetComponent(GameComponentsLookup.GameNamespaceAnyTestEvent3RemovedListener); } }
    public bool hasGameNamespaceAnyTestEvent3RemovedListener { get { return HasComponent(GameComponentsLookup.GameNamespaceAnyTestEvent3RemovedListener); } }

    public void AddGameNamespaceAnyTestEvent3RemovedListener(System.Collections.Generic.List<IGameNamespaceAnyTestEvent3RemovedListener> newValue)
    {
        var index = GameComponentsLookup.GameNamespaceAnyTestEvent3RemovedListener;
        var component = (GameNamespaceAnyTestEvent3RemovedListenerComponent)CreateComponent(index, typeof(GameNamespaceAnyTestEvent3RemovedListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceGameNamespaceAnyTestEvent3RemovedListener(System.Collections.Generic.List<IGameNamespaceAnyTestEvent3RemovedListener> newValue)
    {
        var index = GameComponentsLookup.GameNamespaceAnyTestEvent3RemovedListener;
        var component = (GameNamespaceAnyTestEvent3RemovedListenerComponent)CreateComponent(index, typeof(GameNamespaceAnyTestEvent3RemovedListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveGameNamespaceAnyTestEvent3RemovedListener()
    {
        RemoveComponent(GameComponentsLookup.GameNamespaceAnyTestEvent3RemovedListener);
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherGameNamespaceAnyTestEvent3RemovedListener;

    public static Entitas.IMatcher<GameEntity> GameNamespaceAnyTestEvent3RemovedListener
    {
        get
        {
            if (_matcherGameNamespaceAnyTestEvent3RemovedListener == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.GameNamespaceAnyTestEvent3RemovedListener);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherGameNamespaceAnyTestEvent3RemovedListener = matcher;
            }

            return _matcherGameNamespaceAnyTestEvent3RemovedListener;
        }
    }
}


// GameGameNamespaceAnyTestEvent3RemovedListenerComponentEvent.g.cs
public partial class GameEntity
{
    public void AddGameNamespaceAnyTestEvent3RemovedListener(IGameNamespaceAnyTestEvent3RemovedListener value)
    {
        var listeners = hasGameNamespaceAnyTestEvent3RemovedListener
            ? gameNamespaceAnyTestEvent3RemovedListener.value
            : new System.Collections.Generic.List<IGameNamespaceAnyTestEvent3RemovedListener>();
        listeners.Add(value);
        ReplaceGameNamespaceAnyTestEvent3RemovedListener(listeners);
    }

    public void RemoveGameNamespaceAnyTestEvent3RemovedListener(IGameNamespaceAnyTestEvent3RemovedListener value, bool removeComponentWhenEmpty = true)
    {
        var listeners = gameNamespaceAnyTestEvent3RemovedListener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0)
        {
            RemoveGameNamespaceAnyTestEvent3RemovedListener();
        }
        else
        {
            ReplaceGameNamespaceAnyTestEvent3RemovedListener(listeners);
        }
    }
}


// GameMatcher.g.cs
public sealed partial class GameMatcher 
{
    public static Entitas.IAllOfMatcher<GameEntity> AllOf(params int[] indices) 
    {
        return Entitas.Matcher<GameEntity>.AllOf(indices);
    }

    public static Entitas.IAllOfMatcher<GameEntity> AllOf(params Entitas.IMatcher<GameEntity>[] matchers)
    {
        return Entitas.Matcher<GameEntity>.AllOf(matchers);
    }

    public static Entitas.IAnyOfMatcher<GameEntity> AnyOf(params int[] indices)
    {
        return Entitas.Matcher<GameEntity>.AnyOf(indices);
    }

    public static Entitas.IAnyOfMatcher<GameEntity> AnyOf(params Entitas.IMatcher<GameEntity>[] matchers)
    {
        return Entitas.Matcher<GameEntity>.AnyOf(matchers);
    }
}


// GameNamespaceAnyTestEvent2ListenerComponent.g.cs
public partial class GameEntity
{
    public NamespaceAnyTestEvent2ListenerComponent namespaceAnyTestEvent2Listener { get { return (NamespaceAnyTestEvent2ListenerComponent)GetComponent(GameComponentsLookup.NamespaceAnyTestEvent2Listener); } }
    public bool hasNamespaceAnyTestEvent2Listener { get { return HasComponent(GameComponentsLookup.NamespaceAnyTestEvent2Listener); } }

    public void AddNamespaceAnyTestEvent2Listener(System.Collections.Generic.List<INamespaceAnyTestEvent2Listener> newValue)
    {
        var index = GameComponentsLookup.NamespaceAnyTestEvent2Listener;
        var component = (NamespaceAnyTestEvent2ListenerComponent)CreateComponent(index, typeof(NamespaceAnyTestEvent2ListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceNamespaceAnyTestEvent2Listener(System.Collections.Generic.List<INamespaceAnyTestEvent2Listener> newValue)
    {
        var index = GameComponentsLookup.NamespaceAnyTestEvent2Listener;
        var component = (NamespaceAnyTestEvent2ListenerComponent)CreateComponent(index, typeof(NamespaceAnyTestEvent2ListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveNamespaceAnyTestEvent2Listener()
    {
        RemoveComponent(GameComponentsLookup.NamespaceAnyTestEvent2Listener);
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherNamespaceAnyTestEvent2Listener;

    public static Entitas.IMatcher<GameEntity> NamespaceAnyTestEvent2Listener
    {
        get
        {
            if (_matcherNamespaceAnyTestEvent2Listener == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.NamespaceAnyTestEvent2Listener);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherNamespaceAnyTestEvent2Listener = matcher;
            }

            return _matcherNamespaceAnyTestEvent2Listener;
        }
    }
}


// GameNamespaceAnyTestEvent2ListenerComponentEvent.g.cs
public partial class GameEntity
{
    public void AddNamespaceAnyTestEvent2Listener(INamespaceAnyTestEvent2Listener value)
    {
        var listeners = hasNamespaceAnyTestEvent2Listener
            ? namespaceAnyTestEvent2Listener.value
            : new System.Collections.Generic.List<INamespaceAnyTestEvent2Listener>();
        listeners.Add(value);
        ReplaceNamespaceAnyTestEvent2Listener(listeners);
    }

    public void RemoveNamespaceAnyTestEvent2Listener(INamespaceAnyTestEvent2Listener value, bool removeComponentWhenEmpty = true)
    {
        var listeners = namespaceAnyTestEvent2Listener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0)
        {
            RemoveNamespaceAnyTestEvent2Listener();
        }
        else
        {
            ReplaceNamespaceAnyTestEvent2Listener(listeners);
        }
    }
}


// GameNamespaceAnyTestEvent2RemovedListenerComponent.g.cs
public partial class GameEntity
{
    public NamespaceAnyTestEvent2RemovedListenerComponent namespaceAnyTestEvent2RemovedListener { get { return (NamespaceAnyTestEvent2RemovedListenerComponent)GetComponent(GameComponentsLookup.NamespaceAnyTestEvent2RemovedListener); } }
    public bool hasNamespaceAnyTestEvent2RemovedListener { get { return HasComponent(GameComponentsLookup.NamespaceAnyTestEvent2RemovedListener); } }

    public void AddNamespaceAnyTestEvent2RemovedListener(System.Collections.Generic.List<INamespaceAnyTestEvent2RemovedListener> newValue)
    {
        var index = GameComponentsLookup.NamespaceAnyTestEvent2RemovedListener;
        var component = (NamespaceAnyTestEvent2RemovedListenerComponent)CreateComponent(index, typeof(NamespaceAnyTestEvent2RemovedListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceNamespaceAnyTestEvent2RemovedListener(System.Collections.Generic.List<INamespaceAnyTestEvent2RemovedListener> newValue)
    {
        var index = GameComponentsLookup.NamespaceAnyTestEvent2RemovedListener;
        var component = (NamespaceAnyTestEvent2RemovedListenerComponent)CreateComponent(index, typeof(NamespaceAnyTestEvent2RemovedListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveNamespaceAnyTestEvent2RemovedListener()
    {
        RemoveComponent(GameComponentsLookup.NamespaceAnyTestEvent2RemovedListener);
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherNamespaceAnyTestEvent2RemovedListener;

    public static Entitas.IMatcher<GameEntity> NamespaceAnyTestEvent2RemovedListener
    {
        get
        {
            if (_matcherNamespaceAnyTestEvent2RemovedListener == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.NamespaceAnyTestEvent2RemovedListener);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherNamespaceAnyTestEvent2RemovedListener = matcher;
            }

            return _matcherNamespaceAnyTestEvent2RemovedListener;
        }
    }
}


// GameNamespaceAnyTestEvent2RemovedListenerComponentEvent.g.cs
public partial class GameEntity
{
    public void AddNamespaceAnyTestEvent2RemovedListener(INamespaceAnyTestEvent2RemovedListener value)
    {
        var listeners = hasNamespaceAnyTestEvent2RemovedListener
            ? namespaceAnyTestEvent2RemovedListener.value
            : new System.Collections.Generic.List<INamespaceAnyTestEvent2RemovedListener>();
        listeners.Add(value);
        ReplaceNamespaceAnyTestEvent2RemovedListener(listeners);
    }

    public void RemoveNamespaceAnyTestEvent2RemovedListener(INamespaceAnyTestEvent2RemovedListener value, bool removeComponentWhenEmpty = true)
    {
        var listeners = namespaceAnyTestEvent2RemovedListener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0)
        {
            RemoveNamespaceAnyTestEvent2RemovedListener();
        }
        else
        {
            ReplaceNamespaceAnyTestEvent2RemovedListener(listeners);
        }
    }
}


// GameNamespaceAnyTestEvent3EventSystem.g.cs
public sealed class GameNamespaceAnyTestEvent3EventSystem : Entitas.ReactiveSystem<GameEntity>
{
    readonly Entitas.IGroup<GameEntity> _listeners;
    readonly System.Collections.Generic.List<GameEntity> _entityBuffer;
    readonly System.Collections.Generic.List<IGameNamespaceAnyTestEvent3Listener> _listenerBuffer;

    public GameNamespaceAnyTestEvent3EventSystem(Contexts contexts) : base(contexts.game)
    {
        _listeners = contexts.game.GetGroup(GameMatcher.GameNamespaceAnyTestEvent3Listener);
        _entityBuffer = new System.Collections.Generic.List<GameEntity>();
        _listenerBuffer = new System.Collections.Generic.List<IGameNamespaceAnyTestEvent3Listener>();
    }

    protected override Entitas.ICollector<GameEntity> GetTrigger(Entitas.IContext<GameEntity> context)
    {
        return Entitas.CollectorContextExtension.CreateCollector(
            context, Entitas.TriggerOnEventMatcherExtension.Added(GameMatcher.NamespaceTestEvent3)
        );
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasNamespaceTestEvent3;
    }

    protected override void Execute(System.Collections.Generic.List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var component = e.namespaceTestEvent3;
            foreach (var listenerEntity in _listeners.GetEntities(_entityBuffer))
            {
                _listenerBuffer.Clear();
                _listenerBuffer.AddRange(listenerEntity.gameNamespaceAnyTestEvent3Listener.value);
                foreach (var listener in _listenerBuffer)
                {
                    listener.OnNamespaceAnyTestEvent3(e, component.Value);
                }
            }
        }
    }
}


// GameNamespaceAnyTestEvent3ListenerComponent.g.cs
[Entitas.CodeGeneration.Attributes.DontGenerate]
public sealed class GameNamespaceAnyTestEvent3ListenerComponent : Entitas.IComponent
{
    public System.Collections.Generic.List<IGameNamespaceAnyTestEvent3Listener> value;
}


// GameNamespaceAnyTestEvent3RemovedEventSystem.g.cs
public sealed class GameNamespaceAnyTestEvent3RemovedEventSystem : Entitas.ReactiveSystem<GameEntity>
{
    readonly Entitas.IGroup<GameEntity> _listeners;
    readonly System.Collections.Generic.List<GameEntity> _entityBuffer;
    readonly System.Collections.Generic.List<IGameNamespaceAnyTestEvent3RemovedListener> _listenerBuffer;

    public GameNamespaceAnyTestEvent3RemovedEventSystem(Contexts contexts) : base(contexts.game)
    {
        _listeners = contexts.game.GetGroup(GameMatcher.GameNamespaceAnyTestEvent3RemovedListener);
        _entityBuffer = new System.Collections.Generic.List<GameEntity>();
        _listenerBuffer = new System.Collections.Generic.List<IGameNamespaceAnyTestEvent3RemovedListener>();
    }

    protected override Entitas.ICollector<GameEntity> GetTrigger(Entitas.IContext<GameEntity> context)
    {
        return Entitas.CollectorContextExtension.CreateCollector(
            context, Entitas.TriggerOnEventMatcherExtension.Removed(GameMatcher.NamespaceTestEvent3)
        );
    }

    protected override bool Filter(GameEntity entity)
    {
        return !entity.hasNamespaceTestEvent3;
    }

    protected override void Execute(System.Collections.Generic.List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            
            foreach (var listenerEntity in _listeners.GetEntities(_entityBuffer))
            {
                _listenerBuffer.Clear();
                _listenerBuffer.AddRange(listenerEntity.gameNamespaceAnyTestEvent3RemovedListener.value);
                foreach (var listener in _listenerBuffer)
                {
                    listener.OnNamespaceAnyTestEvent3Removed(e);
                }
            }
        }
    }
}


// GameNamespaceAnyTestEvent3RemovedListenerComponent.g.cs
[Entitas.CodeGeneration.Attributes.DontGenerate]
public sealed class GameNamespaceAnyTestEvent3RemovedListenerComponent : Entitas.IComponent
{
    public System.Collections.Generic.List<IGameNamespaceAnyTestEvent3RemovedListener> value;
}


// GameNamespaceTestEvent2Component.g.cs
public partial class GameEntity
{
    public Namespace.TestEvent2Component namespaceTestEvent2 { get { return (Namespace.TestEvent2Component)GetComponent(GameComponentsLookup.NamespaceTestEvent2); } }
    public bool hasNamespaceTestEvent2 { get { return HasComponent(GameComponentsLookup.NamespaceTestEvent2); } }

    public void AddNamespaceTestEvent2(string newValue)
    {
        var index = GameComponentsLookup.NamespaceTestEvent2;
        var component = (Namespace.TestEvent2Component)CreateComponent(index, typeof(Namespace.TestEvent2Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceNamespaceTestEvent2(string newValue)
    {
        var index = GameComponentsLookup.NamespaceTestEvent2;
        var component = (Namespace.TestEvent2Component)CreateComponent(index, typeof(Namespace.TestEvent2Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveNamespaceTestEvent2()
    {
        RemoveComponent(GameComponentsLookup.NamespaceTestEvent2);
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherNamespaceTestEvent2;

    public static Entitas.IMatcher<GameEntity> NamespaceTestEvent2
    {
        get
        {
            if (_matcherNamespaceTestEvent2 == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.NamespaceTestEvent2);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherNamespaceTestEvent2 = matcher;
            }

            return _matcherNamespaceTestEvent2;
        }
    }
}


// GameNamespaceTestEvent3Component.g.cs
public partial class GameEntity
{
    public Namespace.TestEvent3Component namespaceTestEvent3 { get { return (Namespace.TestEvent3Component)GetComponent(GameComponentsLookup.NamespaceTestEvent3); } }
    public bool hasNamespaceTestEvent3 { get { return HasComponent(GameComponentsLookup.NamespaceTestEvent3); } }

    public void AddNamespaceTestEvent3(string newValue)
    {
        var index = GameComponentsLookup.NamespaceTestEvent3;
        var component = (Namespace.TestEvent3Component)CreateComponent(index, typeof(Namespace.TestEvent3Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceNamespaceTestEvent3(string newValue)
    {
        var index = GameComponentsLookup.NamespaceTestEvent3;
        var component = (Namespace.TestEvent3Component)CreateComponent(index, typeof(Namespace.TestEvent3Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveNamespaceTestEvent3()
    {
        RemoveComponent(GameComponentsLookup.NamespaceTestEvent3);
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherNamespaceTestEvent3;

    public static Entitas.IMatcher<GameEntity> NamespaceTestEvent3
    {
        get
        {
            if (_matcherNamespaceTestEvent3 == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.NamespaceTestEvent3);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherNamespaceTestEvent3 = matcher;
            }

            return _matcherNamespaceTestEvent3;
        }
    }
}


// GameTestEventComponent.g.cs
public partial class GameEntity
{
    public TestEventComponent testEvent { get { return (TestEventComponent)GetComponent(GameComponentsLookup.TestEvent); } }
    public bool hasTestEvent { get { return HasComponent(GameComponentsLookup.TestEvent); } }

    public void AddTestEvent(string newValue)
    {
        var index = GameComponentsLookup.TestEvent;
        var component = (TestEventComponent)CreateComponent(index, typeof(TestEventComponent));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestEvent(string newValue)
    {
        var index = GameComponentsLookup.TestEvent;
        var component = (TestEventComponent)CreateComponent(index, typeof(TestEventComponent));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestEvent()
    {
        RemoveComponent(GameComponentsLookup.TestEvent);
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherTestEvent;

    public static Entitas.IMatcher<GameEntity> TestEvent
    {
        get
        {
            if (_matcherTestEvent == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.TestEvent);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherTestEvent = matcher;
            }

            return _matcherTestEvent;
        }
    }
}


// GameTestEventListenerComponent.g.cs
public partial class GameEntity
{
    public TestEventListenerComponent testEventListener { get { return (TestEventListenerComponent)GetComponent(GameComponentsLookup.TestEventListener); } }
    public bool hasTestEventListener { get { return HasComponent(GameComponentsLookup.TestEventListener); } }

    public void AddTestEventListener(System.Collections.Generic.List<ITestEventListener> newValue)
    {
        var index = GameComponentsLookup.TestEventListener;
        var component = (TestEventListenerComponent)CreateComponent(index, typeof(TestEventListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestEventListener(System.Collections.Generic.List<ITestEventListener> newValue)
    {
        var index = GameComponentsLookup.TestEventListener;
        var component = (TestEventListenerComponent)CreateComponent(index, typeof(TestEventListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestEventListener()
    {
        RemoveComponent(GameComponentsLookup.TestEventListener);
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherTestEventListener;

    public static Entitas.IMatcher<GameEntity> TestEventListener
    {
        get
        {
            if (_matcherTestEventListener == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.TestEventListener);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherTestEventListener = matcher;
            }

            return _matcherTestEventListener;
        }
    }
}


// GameTestEventListenerComponentEvent.g.cs
public partial class GameEntity
{
    public void AddTestEventListener(ITestEventListener value)
    {
        var listeners = hasTestEventListener
            ? testEventListener.value
            : new System.Collections.Generic.List<ITestEventListener>();
        listeners.Add(value);
        ReplaceTestEventListener(listeners);
    }

    public void RemoveTestEventListener(ITestEventListener value, bool removeComponentWhenEmpty = true)
    {
        var listeners = testEventListener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0)
        {
            RemoveTestEventListener();
        }
        else
        {
            ReplaceTestEventListener(listeners);
        }
    }
}


// GameTestEventRemovedListenerComponent.g.cs
public partial class GameEntity
{
    public TestEventRemovedListenerComponent testEventRemovedListener { get { return (TestEventRemovedListenerComponent)GetComponent(GameComponentsLookup.TestEventRemovedListener); } }
    public bool hasTestEventRemovedListener { get { return HasComponent(GameComponentsLookup.TestEventRemovedListener); } }

    public void AddTestEventRemovedListener(System.Collections.Generic.List<ITestEventRemovedListener> newValue)
    {
        var index = GameComponentsLookup.TestEventRemovedListener;
        var component = (TestEventRemovedListenerComponent)CreateComponent(index, typeof(TestEventRemovedListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestEventRemovedListener(System.Collections.Generic.List<ITestEventRemovedListener> newValue)
    {
        var index = GameComponentsLookup.TestEventRemovedListener;
        var component = (TestEventRemovedListenerComponent)CreateComponent(index, typeof(TestEventRemovedListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestEventRemovedListener()
    {
        RemoveComponent(GameComponentsLookup.TestEventRemovedListener);
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherTestEventRemovedListener;

    public static Entitas.IMatcher<GameEntity> TestEventRemovedListener
    {
        get
        {
            if (_matcherTestEventRemovedListener == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.TestEventRemovedListener);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherTestEventRemovedListener = matcher;
            }

            return _matcherTestEventRemovedListener;
        }
    }
}


// GameTestEventRemovedListenerComponentEvent.g.cs
public partial class GameEntity
{
    public void AddTestEventRemovedListener(ITestEventRemovedListener value)
    {
        var listeners = hasTestEventRemovedListener
            ? testEventRemovedListener.value
            : new System.Collections.Generic.List<ITestEventRemovedListener>();
        listeners.Add(value);
        ReplaceTestEventRemovedListener(listeners);
    }

    public void RemoveTestEventRemovedListener(ITestEventRemovedListener value, bool removeComponentWhenEmpty = true)
    {
        var listeners = testEventRemovedListener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0)
        {
            RemoveTestEventRemovedListener();
        }
        else
        {
            ReplaceTestEventRemovedListener(listeners);
        }
    }
}


// IAnyTestEventListener.g.cs
public interface IAnyTestEventListener
{
    void OnAnyTestEvent(GameEntity entity, string value);
}


// IAnyTestEventRemovedListener.g.cs
public interface IAnyTestEventRemovedListener
{
    void OnAnyTestEventRemoved(GameEntity entity);
}


// IGameNamespaceAnyTestEvent3Listener.g.cs
public interface IGameNamespaceAnyTestEvent3Listener
{
    void OnNamespaceAnyTestEvent3(GameEntity entity, string value);
}


// IGameNamespaceAnyTestEvent3RemovedListener.g.cs
public interface IGameNamespaceAnyTestEvent3RemovedListener
{
    void OnNamespaceAnyTestEvent3Removed(GameEntity entity);
}


// IInputNamespaceAnyTestEvent3Listener.g.cs
public interface IInputNamespaceAnyTestEvent3Listener
{
    void OnNamespaceAnyTestEvent3(InputEntity entity, string value);
}


// IInputNamespaceAnyTestEvent3RemovedListener.g.cs
public interface IInputNamespaceAnyTestEvent3RemovedListener
{
    void OnNamespaceAnyTestEvent3Removed(InputEntity entity);
}


// INamespaceAnyTestEvent2Listener.g.cs
public interface INamespaceAnyTestEvent2Listener
{
    void OnNamespaceAnyTestEvent2(GameEntity entity, string value);
}


// INamespaceAnyTestEvent2RemovedListener.g.cs
public interface INamespaceAnyTestEvent2RemovedListener
{
    void OnNamespaceAnyTestEvent2Removed(GameEntity entity);
}


// INamespaceTestEvent3Entity.g.cs
public partial interface INamespaceTestEvent3Entity
{
    Namespace.TestEvent3Component namespaceTestEvent3 { get; }
    bool hasNamespaceTestEvent3 { get; }

    void AddNamespaceTestEvent3(string newValue);
    void ReplaceNamespaceTestEvent3(string newValue);
    void RemoveNamespaceTestEvent3();
}

public partial class GameEntity : INamespaceTestEvent3Entity { }

public partial class InputEntity : INamespaceTestEvent3Entity { }


// InputCleanupSystems.g.cs
public sealed class InputCleanupSystems : Feature
{
    public InputCleanupSystems(Contexts contexts)
    {

    }
}


// InputComponentsLookup.g.cs
public static class InputComponentsLookup
{
    public const int NamespaceTestEvent3 = 0;
    public const int InputNamespaceAnyTestEvent3Listener = 1;
    public const int InputNamespaceAnyTestEvent3RemovedListener = 2;

    public const int TotalComponents = 3;

    public static readonly string[] componentNames = 
    {
        "NamespaceTestEvent3",
        "InputNamespaceAnyTestEvent3Listener",
        "InputNamespaceAnyTestEvent3RemovedListener"
    };

    public static readonly System.Type[] componentTypes = 
    {
        typeof(Namespace.TestEvent3Component),
        typeof(InputNamespaceAnyTestEvent3ListenerComponent),
        typeof(InputNamespaceAnyTestEvent3RemovedListenerComponent)
    };
}


// InputContext.g.cs
public sealed partial class InputContext : Entitas.Context<InputEntity>
{
    public InputContext()
        : base(
            InputComponentsLookup.TotalComponents,
            0,
            new Entitas.ContextInfo(
                "Input",
                InputComponentsLookup.componentNames,
                InputComponentsLookup.componentTypes
            ),
            (entity) =>

#if (ENTITAS_FAST_AND_UNSAFE)
                new Entitas.UnsafeAERC(),
#else
                new Entitas.SafeAERC(entity),
#endif
            () => new InputEntity()
        ) 
    {
    }
}


// InputEntity.g.cs
public sealed partial class InputEntity : Entitas.Entity
{
}


// InputEventSystems.g.cs
public sealed class InputEventSystems : Feature
{
    public InputEventSystems(Contexts contexts)
    {
        Add(new InputNamespaceAnyTestEvent3EventSystem(contexts)); // priority: 0
        Add(new InputNamespaceAnyTestEvent3RemovedEventSystem(contexts)); // priority: 0
    }
}


// InputInputNamespaceAnyTestEvent3ListenerComponent.g.cs
public partial class InputEntity
{
    public InputNamespaceAnyTestEvent3ListenerComponent inputNamespaceAnyTestEvent3Listener { get { return (InputNamespaceAnyTestEvent3ListenerComponent)GetComponent(InputComponentsLookup.InputNamespaceAnyTestEvent3Listener); } }
    public bool hasInputNamespaceAnyTestEvent3Listener { get { return HasComponent(InputComponentsLookup.InputNamespaceAnyTestEvent3Listener); } }

    public void AddInputNamespaceAnyTestEvent3Listener(System.Collections.Generic.List<IInputNamespaceAnyTestEvent3Listener> newValue)
    {
        var index = InputComponentsLookup.InputNamespaceAnyTestEvent3Listener;
        var component = (InputNamespaceAnyTestEvent3ListenerComponent)CreateComponent(index, typeof(InputNamespaceAnyTestEvent3ListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceInputNamespaceAnyTestEvent3Listener(System.Collections.Generic.List<IInputNamespaceAnyTestEvent3Listener> newValue)
    {
        var index = InputComponentsLookup.InputNamespaceAnyTestEvent3Listener;
        var component = (InputNamespaceAnyTestEvent3ListenerComponent)CreateComponent(index, typeof(InputNamespaceAnyTestEvent3ListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveInputNamespaceAnyTestEvent3Listener()
    {
        RemoveComponent(InputComponentsLookup.InputNamespaceAnyTestEvent3Listener);
    }
}

public sealed partial class InputMatcher
{
    static Entitas.IMatcher<InputEntity> _matcherInputNamespaceAnyTestEvent3Listener;

    public static Entitas.IMatcher<InputEntity> InputNamespaceAnyTestEvent3Listener
    {
        get
        {
            if (_matcherInputNamespaceAnyTestEvent3Listener == null)
            {
                var matcher = (Entitas.Matcher<InputEntity>)Entitas.Matcher<InputEntity>.AllOf(InputComponentsLookup.InputNamespaceAnyTestEvent3Listener);
                matcher.componentNames = InputComponentsLookup.componentNames;
                _matcherInputNamespaceAnyTestEvent3Listener = matcher;
            }

            return _matcherInputNamespaceAnyTestEvent3Listener;
        }
    }
}


// InputInputNamespaceAnyTestEvent3ListenerComponentEvent.g.cs
public partial class InputEntity
{
    public void AddInputNamespaceAnyTestEvent3Listener(IInputNamespaceAnyTestEvent3Listener value)
    {
        var listeners = hasInputNamespaceAnyTestEvent3Listener
            ? inputNamespaceAnyTestEvent3Listener.value
            : new System.Collections.Generic.List<IInputNamespaceAnyTestEvent3Listener>();
        listeners.Add(value);
        ReplaceInputNamespaceAnyTestEvent3Listener(listeners);
    }

    public void RemoveInputNamespaceAnyTestEvent3Listener(IInputNamespaceAnyTestEvent3Listener value, bool removeComponentWhenEmpty = true)
    {
        var listeners = inputNamespaceAnyTestEvent3Listener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0)
        {
            RemoveInputNamespaceAnyTestEvent3Listener();
        }
        else
        {
            ReplaceInputNamespaceAnyTestEvent3Listener(listeners);
        }
    }
}


// InputInputNamespaceAnyTestEvent3RemovedListenerComponent.g.cs
public partial class InputEntity
{
    public InputNamespaceAnyTestEvent3RemovedListenerComponent inputNamespaceAnyTestEvent3RemovedListener { get { return (InputNamespaceAnyTestEvent3RemovedListenerComponent)GetComponent(InputComponentsLookup.InputNamespaceAnyTestEvent3RemovedListener); } }
    public bool hasInputNamespaceAnyTestEvent3RemovedListener { get { return HasComponent(InputComponentsLookup.InputNamespaceAnyTestEvent3RemovedListener); } }

    public void AddInputNamespaceAnyTestEvent3RemovedListener(System.Collections.Generic.List<IInputNamespaceAnyTestEvent3RemovedListener> newValue)
    {
        var index = InputComponentsLookup.InputNamespaceAnyTestEvent3RemovedListener;
        var component = (InputNamespaceAnyTestEvent3RemovedListenerComponent)CreateComponent(index, typeof(InputNamespaceAnyTestEvent3RemovedListenerComponent));
        component.value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceInputNamespaceAnyTestEvent3RemovedListener(System.Collections.Generic.List<IInputNamespaceAnyTestEvent3RemovedListener> newValue)
    {
        var index = InputComponentsLookup.InputNamespaceAnyTestEvent3RemovedListener;
        var component = (InputNamespaceAnyTestEvent3RemovedListenerComponent)CreateComponent(index, typeof(InputNamespaceAnyTestEvent3RemovedListenerComponent));
        component.value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveInputNamespaceAnyTestEvent3RemovedListener()
    {
        RemoveComponent(InputComponentsLookup.InputNamespaceAnyTestEvent3RemovedListener);
    }
}

public sealed partial class InputMatcher
{
    static Entitas.IMatcher<InputEntity> _matcherInputNamespaceAnyTestEvent3RemovedListener;

    public static Entitas.IMatcher<InputEntity> InputNamespaceAnyTestEvent3RemovedListener
    {
        get
        {
            if (_matcherInputNamespaceAnyTestEvent3RemovedListener == null)
            {
                var matcher = (Entitas.Matcher<InputEntity>)Entitas.Matcher<InputEntity>.AllOf(InputComponentsLookup.InputNamespaceAnyTestEvent3RemovedListener);
                matcher.componentNames = InputComponentsLookup.componentNames;
                _matcherInputNamespaceAnyTestEvent3RemovedListener = matcher;
            }

            return _matcherInputNamespaceAnyTestEvent3RemovedListener;
        }
    }
}


// InputInputNamespaceAnyTestEvent3RemovedListenerComponentEvent.g.cs
public partial class InputEntity
{
    public void AddInputNamespaceAnyTestEvent3RemovedListener(IInputNamespaceAnyTestEvent3RemovedListener value)
    {
        var listeners = hasInputNamespaceAnyTestEvent3RemovedListener
            ? inputNamespaceAnyTestEvent3RemovedListener.value
            : new System.Collections.Generic.List<IInputNamespaceAnyTestEvent3RemovedListener>();
        listeners.Add(value);
        ReplaceInputNamespaceAnyTestEvent3RemovedListener(listeners);
    }

    public void RemoveInputNamespaceAnyTestEvent3RemovedListener(IInputNamespaceAnyTestEvent3RemovedListener value, bool removeComponentWhenEmpty = true)
    {
        var listeners = inputNamespaceAnyTestEvent3RemovedListener.value;
        listeners.Remove(value);
        if (removeComponentWhenEmpty && listeners.Count == 0)
        {
            RemoveInputNamespaceAnyTestEvent3RemovedListener();
        }
        else
        {
            ReplaceInputNamespaceAnyTestEvent3RemovedListener(listeners);
        }
    }
}


// InputMatcher.g.cs
public sealed partial class InputMatcher 
{
    public static Entitas.IAllOfMatcher<InputEntity> AllOf(params int[] indices) 
    {
        return Entitas.Matcher<InputEntity>.AllOf(indices);
    }

    public static Entitas.IAllOfMatcher<InputEntity> AllOf(params Entitas.IMatcher<InputEntity>[] matchers)
    {
        return Entitas.Matcher<InputEntity>.AllOf(matchers);
    }

    public static Entitas.IAnyOfMatcher<InputEntity> AnyOf(params int[] indices)
    {
        return Entitas.Matcher<InputEntity>.AnyOf(indices);
    }

    public static Entitas.IAnyOfMatcher<InputEntity> AnyOf(params Entitas.IMatcher<InputEntity>[] matchers)
    {
        return Entitas.Matcher<InputEntity>.AnyOf(matchers);
    }
}


// InputNamespaceAnyTestEvent3EventSystem.g.cs
public sealed class InputNamespaceAnyTestEvent3EventSystem : Entitas.ReactiveSystem<InputEntity>
{
    readonly Entitas.IGroup<InputEntity> _listeners;
    readonly System.Collections.Generic.List<InputEntity> _entityBuffer;
    readonly System.Collections.Generic.List<IInputNamespaceAnyTestEvent3Listener> _listenerBuffer;

    public InputNamespaceAnyTestEvent3EventSystem(Contexts contexts) : base(contexts.input)
    {
        _listeners = contexts.input.GetGroup(InputMatcher.InputNamespaceAnyTestEvent3Listener);
        _entityBuffer = new System.Collections.Generic.List<InputEntity>();
        _listenerBuffer = new System.Collections.Generic.List<IInputNamespaceAnyTestEvent3Listener>();
    }

    protected override Entitas.ICollector<InputEntity> GetTrigger(Entitas.IContext<InputEntity> context)
    {
        return Entitas.CollectorContextExtension.CreateCollector(
            context, Entitas.TriggerOnEventMatcherExtension.Added(InputMatcher.NamespaceTestEvent3)
        );
    }

    protected override bool Filter(InputEntity entity)
    {
        return entity.hasNamespaceTestEvent3;
    }

    protected override void Execute(System.Collections.Generic.List<InputEntity> entities)
    {
        foreach (var e in entities)
        {
            var component = e.namespaceTestEvent3;
            foreach (var listenerEntity in _listeners.GetEntities(_entityBuffer))
            {
                _listenerBuffer.Clear();
                _listenerBuffer.AddRange(listenerEntity.inputNamespaceAnyTestEvent3Listener.value);
                foreach (var listener in _listenerBuffer)
                {
                    listener.OnNamespaceAnyTestEvent3(e, component.Value);
                }
            }
        }
    }
}


// InputNamespaceAnyTestEvent3ListenerComponent.g.cs
[Entitas.CodeGeneration.Attributes.DontGenerate]
public sealed class InputNamespaceAnyTestEvent3ListenerComponent : Entitas.IComponent
{
    public System.Collections.Generic.List<IInputNamespaceAnyTestEvent3Listener> value;
}


// InputNamespaceAnyTestEvent3RemovedEventSystem.g.cs
public sealed class InputNamespaceAnyTestEvent3RemovedEventSystem : Entitas.ReactiveSystem<InputEntity>
{
    readonly Entitas.IGroup<InputEntity> _listeners;
    readonly System.Collections.Generic.List<InputEntity> _entityBuffer;
    readonly System.Collections.Generic.List<IInputNamespaceAnyTestEvent3RemovedListener> _listenerBuffer;

    public InputNamespaceAnyTestEvent3RemovedEventSystem(Contexts contexts) : base(contexts.input)
    {
        _listeners = contexts.input.GetGroup(InputMatcher.InputNamespaceAnyTestEvent3RemovedListener);
        _entityBuffer = new System.Collections.Generic.List<InputEntity>();
        _listenerBuffer = new System.Collections.Generic.List<IInputNamespaceAnyTestEvent3RemovedListener>();
    }

    protected override Entitas.ICollector<InputEntity> GetTrigger(Entitas.IContext<InputEntity> context)
    {
        return Entitas.CollectorContextExtension.CreateCollector(
            context, Entitas.TriggerOnEventMatcherExtension.Removed(InputMatcher.NamespaceTestEvent3)
        );
    }

    protected override bool Filter(InputEntity entity)
    {
        return !entity.hasNamespaceTestEvent3;
    }

    protected override void Execute(System.Collections.Generic.List<InputEntity> entities)
    {
        foreach (var e in entities)
        {
            
            foreach (var listenerEntity in _listeners.GetEntities(_entityBuffer))
            {
                _listenerBuffer.Clear();
                _listenerBuffer.AddRange(listenerEntity.inputNamespaceAnyTestEvent3RemovedListener.value);
                foreach (var listener in _listenerBuffer)
                {
                    listener.OnNamespaceAnyTestEvent3Removed(e);
                }
            }
        }
    }
}


// InputNamespaceAnyTestEvent3RemovedListenerComponent.g.cs
[Entitas.CodeGeneration.Attributes.DontGenerate]
public sealed class InputNamespaceAnyTestEvent3RemovedListenerComponent : Entitas.IComponent
{
    public System.Collections.Generic.List<IInputNamespaceAnyTestEvent3RemovedListener> value;
}


// InputNamespaceTestEvent3Component.g.cs
public partial class InputEntity
{
    public Namespace.TestEvent3Component namespaceTestEvent3 { get { return (Namespace.TestEvent3Component)GetComponent(InputComponentsLookup.NamespaceTestEvent3); } }
    public bool hasNamespaceTestEvent3 { get { return HasComponent(InputComponentsLookup.NamespaceTestEvent3); } }

    public void AddNamespaceTestEvent3(string newValue)
    {
        var index = InputComponentsLookup.NamespaceTestEvent3;
        var component = (Namespace.TestEvent3Component)CreateComponent(index, typeof(Namespace.TestEvent3Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceNamespaceTestEvent3(string newValue)
    {
        var index = InputComponentsLookup.NamespaceTestEvent3;
        var component = (Namespace.TestEvent3Component)CreateComponent(index, typeof(Namespace.TestEvent3Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveNamespaceTestEvent3()
    {
        RemoveComponent(InputComponentsLookup.NamespaceTestEvent3);
    }
}

public sealed partial class InputMatcher
{
    static Entitas.IMatcher<InputEntity> _matcherNamespaceTestEvent3;

    public static Entitas.IMatcher<InputEntity> NamespaceTestEvent3
    {
        get
        {
            if (_matcherNamespaceTestEvent3 == null)
            {
                var matcher = (Entitas.Matcher<InputEntity>)Entitas.Matcher<InputEntity>.AllOf(InputComponentsLookup.NamespaceTestEvent3);
                matcher.componentNames = InputComponentsLookup.componentNames;
                _matcherNamespaceTestEvent3 = matcher;
            }

            return _matcherNamespaceTestEvent3;
        }
    }
}


// ITestEventListener.g.cs
public interface ITestEventListener
{
    void OnTestEvent(GameEntity entity, string value);
}


// ITestEventRemovedListener.g.cs
public interface ITestEventRemovedListener
{
    void OnTestEventRemoved(GameEntity entity);
}


// NamespaceAnyTestEvent2EventSystem.g.cs
public sealed class NamespaceAnyTestEvent2EventSystem : Entitas.ReactiveSystem<GameEntity>
{
    readonly Entitas.IGroup<GameEntity> _listeners;
    readonly System.Collections.Generic.List<GameEntity> _entityBuffer;
    readonly System.Collections.Generic.List<INamespaceAnyTestEvent2Listener> _listenerBuffer;

    public NamespaceAnyTestEvent2EventSystem(Contexts contexts) : base(contexts.game)
    {
        _listeners = contexts.game.GetGroup(GameMatcher.NamespaceAnyTestEvent2Listener);
        _entityBuffer = new System.Collections.Generic.List<GameEntity>();
        _listenerBuffer = new System.Collections.Generic.List<INamespaceAnyTestEvent2Listener>();
    }

    protected override Entitas.ICollector<GameEntity> GetTrigger(Entitas.IContext<GameEntity> context)
    {
        return Entitas.CollectorContextExtension.CreateCollector(
            context, Entitas.TriggerOnEventMatcherExtension.Added(GameMatcher.NamespaceTestEvent2)
        );
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasNamespaceTestEvent2;
    }

    protected override void Execute(System.Collections.Generic.List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var component = e.namespaceTestEvent2;
            foreach (var listenerEntity in _listeners.GetEntities(_entityBuffer))
            {
                _listenerBuffer.Clear();
                _listenerBuffer.AddRange(listenerEntity.namespaceAnyTestEvent2Listener.value);
                foreach (var listener in _listenerBuffer)
                {
                    listener.OnNamespaceAnyTestEvent2(e, component.Value);
                }
            }
        }
    }
}


// NamespaceAnyTestEvent2ListenerComponent.g.cs
[Entitas.CodeGeneration.Attributes.DontGenerate]
public sealed class NamespaceAnyTestEvent2ListenerComponent : Entitas.IComponent
{
    public System.Collections.Generic.List<INamespaceAnyTestEvent2Listener> value;
}


// NamespaceAnyTestEvent2RemovedEventSystem.g.cs
public sealed class NamespaceAnyTestEvent2RemovedEventSystem : Entitas.ReactiveSystem<GameEntity>
{
    readonly Entitas.IGroup<GameEntity> _listeners;
    readonly System.Collections.Generic.List<GameEntity> _entityBuffer;
    readonly System.Collections.Generic.List<INamespaceAnyTestEvent2RemovedListener> _listenerBuffer;

    public NamespaceAnyTestEvent2RemovedEventSystem(Contexts contexts) : base(contexts.game)
    {
        _listeners = contexts.game.GetGroup(GameMatcher.NamespaceAnyTestEvent2RemovedListener);
        _entityBuffer = new System.Collections.Generic.List<GameEntity>();
        _listenerBuffer = new System.Collections.Generic.List<INamespaceAnyTestEvent2RemovedListener>();
    }

    protected override Entitas.ICollector<GameEntity> GetTrigger(Entitas.IContext<GameEntity> context)
    {
        return Entitas.CollectorContextExtension.CreateCollector(
            context, Entitas.TriggerOnEventMatcherExtension.Removed(GameMatcher.NamespaceTestEvent2)
        );
    }

    protected override bool Filter(GameEntity entity)
    {
        return !entity.hasNamespaceTestEvent2;
    }

    protected override void Execute(System.Collections.Generic.List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            
            foreach (var listenerEntity in _listeners.GetEntities(_entityBuffer))
            {
                _listenerBuffer.Clear();
                _listenerBuffer.AddRange(listenerEntity.namespaceAnyTestEvent2RemovedListener.value);
                foreach (var listener in _listenerBuffer)
                {
                    listener.OnNamespaceAnyTestEvent2Removed(e);
                }
            }
        }
    }
}


// NamespaceAnyTestEvent2RemovedListenerComponent.g.cs
[Entitas.CodeGeneration.Attributes.DontGenerate]
public sealed class NamespaceAnyTestEvent2RemovedListenerComponent : Entitas.IComponent
{
    public System.Collections.Generic.List<INamespaceAnyTestEvent2RemovedListener> value;
}


// TestEventEventSystem.g.cs
public sealed class TestEventEventSystem : Entitas.ReactiveSystem<GameEntity>
{
    readonly System.Collections.Generic.List<ITestEventListener> _listenerBuffer;

    public TestEventEventSystem(Contexts contexts) : base(contexts.game)
    {
        _listenerBuffer = new System.Collections.Generic.List<ITestEventListener>();
    }

    protected override Entitas.ICollector<GameEntity> GetTrigger(Entitas.IContext<GameEntity> context)
    {
        return Entitas.CollectorContextExtension.CreateCollector(
            context, Entitas.TriggerOnEventMatcherExtension.Added(GameMatcher.TestEvent)
        );
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasTestEvent && entity.hasTestEventListener;
    }

    protected override void Execute(System.Collections.Generic.List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            var component = e.testEvent;
            _listenerBuffer.Clear();
            _listenerBuffer.AddRange(e.testEventListener.value);
            foreach (var listener in _listenerBuffer)
            {
                listener.OnTestEvent(e, component.Value);
            }
        }
    }
}


// TestEventListenerComponent.g.cs
[Entitas.CodeGeneration.Attributes.DontGenerate]
public sealed class TestEventListenerComponent : Entitas.IComponent
{
    public System.Collections.Generic.List<ITestEventListener> value;
}


// TestEventRemovedEventSystem.g.cs
public sealed class TestEventRemovedEventSystem : Entitas.ReactiveSystem<GameEntity>
{
    readonly System.Collections.Generic.List<ITestEventRemovedListener> _listenerBuffer;

    public TestEventRemovedEventSystem(Contexts contexts) : base(contexts.game)
    {
        _listenerBuffer = new System.Collections.Generic.List<ITestEventRemovedListener>();
    }

    protected override Entitas.ICollector<GameEntity> GetTrigger(Entitas.IContext<GameEntity> context)
    {
        return Entitas.CollectorContextExtension.CreateCollector(
            context, Entitas.TriggerOnEventMatcherExtension.Removed(GameMatcher.TestEvent)
        );
    }

    protected override bool Filter(GameEntity entity)
    {
        return !entity.hasTestEvent && entity.hasTestEventRemovedListener;
    }

    protected override void Execute(System.Collections.Generic.List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            
            _listenerBuffer.Clear();
            _listenerBuffer.AddRange(e.testEventRemovedListener.value);
            foreach (var listener in _listenerBuffer)
            {
                listener.OnTestEventRemoved(e);
            }
        }
    }
}


// TestEventRemovedListenerComponent.g.cs
[Entitas.CodeGeneration.Attributes.DontGenerate]
public sealed class TestEventRemovedListenerComponent : Entitas.IComponent
{
    public System.Collections.Generic.List<ITestEventRemovedListener> value;
}
