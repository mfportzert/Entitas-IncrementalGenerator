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

    public GameContext Game { get; set; }

    public Entitas.IContext[] allContexts { get { return new Entitas.IContext [] { Game }; } }

    public Contexts()
    {
        Game = new GameContext();

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
    public const int TestUnique = 0;

    public const int TotalComponents = 1;

    public static readonly string[] componentNames = 
    {
        "TestUnique"
    };

    public static readonly System.Type[] componentTypes = 
    {
        typeof(TestUniqueComponent)
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


// GameTestUniqueComponent.g.cs
public partial class GameContext
{
    public GameEntity testUniqueEntity { get { return GetGroup(GameMatcher.TestUnique).GetSingleEntity(); } }
    public TestUniqueComponent testUnique { get { return testUniqueEntity.testUnique; } }
    public bool hasTestUnique { get { return testUniqueEntity != null; } }

    public GameEntity SetTestUnique(string newValue)
    {
        if (hasTestUnique)
        {
            throw new Entitas.EntitasException("Could not set TestUnique!\n" + this + " already has an entity with TestUniqueComponent!",
                "You should check if the context already has a testUniqueEntity before setting it or use context.ReplaceTestUnique().");
        }
        var entity = CreateEntity();
        entity.AddTestUnique(newValue);
        return entity;
    }

    public void ReplaceTestUnique(string newValue)
    {
        var entity = testUniqueEntity;
        if (entity == null)
        {
            entity = SetTestUnique(newValue);
        }
        else
        {
            entity.ReplaceTestUnique(newValue);
        }
    }

    public void RemoveTestUnique()
    {
        testUniqueEntity.Destroy();
    }
}

public partial class GameEntity
{
    public TestUniqueComponent testUnique { get { return (TestUniqueComponent)GetComponent(GameComponentsLookup.TestUnique); } }
    public bool hasTestUnique { get { return HasComponent(GameComponentsLookup.TestUnique); } }

    public void AddTestUnique(string newValue)
    {
        var index = GameComponentsLookup.TestUnique;
        var component = (TestUniqueComponent)CreateComponent(index, typeof(TestUniqueComponent));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestUnique(string newValue)
    {
        var index = GameComponentsLookup.TestUnique;
        var component = (TestUniqueComponent)CreateComponent(index, typeof(TestUniqueComponent));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestUnique()
    {
        RemoveComponent(GameComponentsLookup.TestUnique);
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherTestUnique;

    public static Entitas.IMatcher<GameEntity> TestUnique
    {
        get
        {
            if (_matcherTestUnique == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.TestUnique);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherTestUnique = matcher;
            }

            return _matcherTestUnique;
        }
    }
}
