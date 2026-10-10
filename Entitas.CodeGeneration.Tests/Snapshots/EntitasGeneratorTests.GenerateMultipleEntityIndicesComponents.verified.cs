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

    public Entitas.IContext[] allContexts { get { return new Entitas.IContext [] { game }; } }

    public Contexts()
    {
        game = new GameContext();

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
    public const int TestMember1 = 0;
    public const int TestMember2 = 1;

    public const int TotalComponents = 2;

    public static readonly string[] componentNames = 
    {
        "TestMember1",
        "TestMember2"
    };

    public static readonly System.Type[] componentTypes = 
    {
        typeof(TestMember1Component),
        typeof(TestMember2Component)
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


// GameEntityIndices.g.cs
public partial class Contexts
{
    public const string TestMember1 = "TestMember1";
    public const string TestMember2 = "TestMember2";

    [Entitas.CodeGeneration.Attributes.PostConstructor]
    public void InitializeEntityIndices()
    {
        game.AddEntityIndex(new Entitas.EntityIndex<GameEntity, string>(
            TestMember1,
            game.GetGroup(GameMatcher.TestMember1),
            (e, c) => ((TestMember1Component)c).Value));

        game.AddEntityIndex(new Entitas.EntityIndex<GameEntity, string>(
            TestMember2,
            game.GetGroup(GameMatcher.TestMember2),
            (e, c) => ((TestMember2Component)c).Value));
    }
}

public static class ContextsExtensions
{
    public static System.Collections.Generic.HashSet<GameEntity> GetEntitiesWithTestMember1(this GameContext context, string Value) {
        return ((Entitas.EntityIndex<GameEntity, string>)context.GetEntityIndex(Contexts.TestMember1)).GetEntities(Value);
    }

    public static System.Collections.Generic.HashSet<GameEntity> GetEntitiesWithTestMember2(this GameContext context, string Value) {
        return ((Entitas.EntityIndex<GameEntity, string>)context.GetEntityIndex(Contexts.TestMember2)).GetEntities(Value);
    }
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


// GameTestMember1Component.g.cs
public partial class GameEntity
{
    public TestMember1Component testMember1 { get { return (TestMember1Component)GetComponent(GameComponentsLookup.TestMember1); } }
    public bool hasTestMember1 { get { return HasComponent(GameComponentsLookup.TestMember1); } }

    public void AddTestMember1(string newValue)
    {
        var index = GameComponentsLookup.TestMember1;
        var component = (TestMember1Component)CreateComponent(index, typeof(TestMember1Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember1(string newValue)
    {
        var index = GameComponentsLookup.TestMember1;
        var component = (TestMember1Component)CreateComponent(index, typeof(TestMember1Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember1()
    {
        RemoveComponent(GameComponentsLookup.TestMember1);
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherTestMember1;

    public static Entitas.IMatcher<GameEntity> TestMember1
    {
        get
        {
            if (_matcherTestMember1 == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.TestMember1);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherTestMember1 = matcher;
            }

            return _matcherTestMember1;
        }
    }
}


// GameTestMember2Component.g.cs
public partial class GameEntity
{
    public TestMember2Component testMember2 { get { return (TestMember2Component)GetComponent(GameComponentsLookup.TestMember2); } }
    public bool hasTestMember2 { get { return HasComponent(GameComponentsLookup.TestMember2); } }

    public void AddTestMember2(string newValue)
    {
        var index = GameComponentsLookup.TestMember2;
        var component = (TestMember2Component)CreateComponent(index, typeof(TestMember2Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember2(string newValue)
    {
        var index = GameComponentsLookup.TestMember2;
        var component = (TestMember2Component)CreateComponent(index, typeof(TestMember2Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember2()
    {
        RemoveComponent(GameComponentsLookup.TestMember2);
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherTestMember2;

    public static Entitas.IMatcher<GameEntity> TestMember2
    {
        get
        {
            if (_matcherTestMember2 == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.TestMember2);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherTestMember2 = matcher;
            }

            return _matcherTestMember2;
        }
    }
}
