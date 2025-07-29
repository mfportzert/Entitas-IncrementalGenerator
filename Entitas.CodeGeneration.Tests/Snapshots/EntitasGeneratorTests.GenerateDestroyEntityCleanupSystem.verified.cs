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


// DestroyTestMemberGameSystem.g.cs
using System.Collections.Generic;
using Entitas;

public sealed class DestroyTestMemberGameSystem : ICleanupSystem
{
    readonly IGroup<GameEntity> _group;
    readonly List<GameEntity> _buffer = new List<GameEntity>();

    public DestroyTestMemberGameSystem(Contexts contexts)
    {
        _group = contexts.game.GetGroup(GameMatcher.TestMember);
    }

    public void Cleanup()
    {
        foreach (var e in _group.GetEntities(_buffer))
        {
            e.Destroy();
        }
    }
}


// GameCleanupSystems.g.cs
public sealed class GameCleanupSystems : Feature
{
    public GameCleanupSystems(Contexts contexts)
    {
        Add(new DestroyTestMemberGameSystem(contexts));
    }
}


// GameComponentsLookup.g.cs
public static class GameComponentsLookup
{
    public const int TestMember = 0;

    public const int TotalComponents = 1;

    public static readonly string[] componentNames = 
    {
        "TestMember"
    };

    public static readonly System.Type[] componentTypes = 
    {
        typeof(TestMemberComponent)
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


// GameTestMemberComponent.g.cs
public partial class GameEntity
{
    public TestMemberComponent testMember { get { return (TestMemberComponent)GetComponent(GameComponentsLookup.TestMember); } }
    public bool hasTestMember { get { return HasComponent(GameComponentsLookup.TestMember); } }

    public void AddTestMember(string newValue)
    {
        var index = GameComponentsLookup.TestMember;
        var component = (TestMemberComponent)CreateComponent(index, typeof(TestMemberComponent));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember(string newValue)
    {
        var index = GameComponentsLookup.TestMember;
        var component = (TestMemberComponent)CreateComponent(index, typeof(TestMemberComponent));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember()
    {
        RemoveComponent(GameComponentsLookup.TestMember);
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherTestMember;

    public static Entitas.IMatcher<GameEntity> TestMember
    {
        get
        {
            if (_matcherTestMember == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.TestMember);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherTestMember = matcher;
            }

            return _matcherTestMember;
        }
    }
}
