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
    public InputContext Input { get; set; }

    public Entitas.IContext[] allContexts { get { return new Entitas.IContext [] { Game, Input }; } }

    public Contexts()
    {
        Game = new GameContext();
        Input = new InputContext();

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
    public const int TestFlag = 0;
    public const int TestMember = 1;

    public const int TotalComponents = 2;

    public static readonly string[] componentNames = 
    {
        "TestFlag",
        "TestMember"
    };

    public static readonly System.Type[] componentTypes = 
    {
        typeof(TestFlagComponent),
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


// GameTestFlagComponent.g.cs
public partial class GameEntity
{
    static readonly TestFlagComponent testFlagComponent = new TestFlagComponent();

    public bool isTestFlag
    {
        get { return HasComponent(GameComponentsLookup.TestFlag); }
        set 
        {
            if (value != isTestFlag)
            {
                var index = GameComponentsLookup.TestFlag;
                if (value)
                {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : testFlagComponent;

                    AddComponent(index, component);
                }
                else
                {
                    RemoveComponent(index);
                }
            }
        }
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherTestFlag;

    public static Entitas.IMatcher<GameEntity> TestFlag
    {
        get
        {
            if (_matcherTestFlag == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.TestFlag);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherTestFlag = matcher;
            }

            return _matcherTestFlag;
        }
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
    public const int TestFlag = 0;
    public const int TestMember = 1;

    public const int TotalComponents = 2;

    public static readonly string[] componentNames = 
    {
        "TestFlag",
        "TestMember"
    };

    public static readonly System.Type[] componentTypes = 
    {
        typeof(TestFlagComponent),
        typeof(TestMemberComponent)
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


// InputTestFlagComponent.g.cs
public partial class InputEntity
{
    static readonly TestFlagComponent testFlagComponent = new TestFlagComponent();

    public bool isTestFlag
    {
        get { return HasComponent(InputComponentsLookup.TestFlag); }
        set 
        {
            if (value != isTestFlag)
            {
                var index = InputComponentsLookup.TestFlag;
                if (value)
                {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : testFlagComponent;

                    AddComponent(index, component);
                }
                else
                {
                    RemoveComponent(index);
                }
            }
        }
    }
}

public sealed partial class InputMatcher
{
    static Entitas.IMatcher<InputEntity> _matcherTestFlag;

    public static Entitas.IMatcher<InputEntity> TestFlag
    {
        get
        {
            if (_matcherTestFlag == null)
            {
                var matcher = (Entitas.Matcher<InputEntity>)Entitas.Matcher<InputEntity>.AllOf(InputComponentsLookup.TestFlag);
                matcher.componentNames = InputComponentsLookup.componentNames;
                _matcherTestFlag = matcher;
            }

            return _matcherTestFlag;
        }
    }
}


// InputTestMemberComponent.g.cs
public partial class InputEntity
{
    public TestMemberComponent testMember { get { return (TestMemberComponent)GetComponent(InputComponentsLookup.TestMember); } }
    public bool hasTestMember { get { return HasComponent(InputComponentsLookup.TestMember); } }

    public void AddTestMember(string newValue)
    {
        var index = InputComponentsLookup.TestMember;
        var component = (TestMemberComponent)CreateComponent(index, typeof(TestMemberComponent));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember(string newValue)
    {
        var index = InputComponentsLookup.TestMember;
        var component = (TestMemberComponent)CreateComponent(index, typeof(TestMemberComponent));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember()
    {
        RemoveComponent(InputComponentsLookup.TestMember);
    }
}

public sealed partial class InputMatcher
{
    static Entitas.IMatcher<InputEntity> _matcherTestMember;

    public static Entitas.IMatcher<InputEntity> TestMember
    {
        get
        {
            if (_matcherTestMember == null)
            {
                var matcher = (Entitas.Matcher<InputEntity>)Entitas.Matcher<InputEntity>.AllOf(InputComponentsLookup.TestMember);
                matcher.componentNames = InputComponentsLookup.componentNames;
                _matcherTestMember = matcher;
            }

            return _matcherTestMember;
        }
    }
}


// ITestFlagEntity.g.cs
public partial interface ITestFlagEntity
{
    bool isTestFlag { get; set; }
}

public partial class GameEntity : ITestFlagEntity { }

public partial class InputEntity : ITestFlagEntity { }


// ITestMemberEntity.g.cs
public partial interface ITestMemberEntity
{
    TestMemberComponent testMember { get; }
    bool hasTestMember { get; }

    void AddTestMember(string newValue);
    void ReplaceTestMember(string newValue);
    void RemoveTestMember();
}

public partial class GameEntity : ITestMemberEntity { }

public partial class InputEntity : ITestMemberEntity { }
