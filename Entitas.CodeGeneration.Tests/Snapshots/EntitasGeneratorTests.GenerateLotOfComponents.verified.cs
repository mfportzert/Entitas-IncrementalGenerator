// BlablaComponentsLookup.g.cs
public static class BlablaComponentsLookup
{
    public const int TestFlag = 0;
    public const int TestMember = 1;
    public const int TestMember2 = 2;
    public const int TestMember3 = 3;
    public const int TestMember4 = 4;
    public const int TestMember5 = 5;
    public const int TestMember6 = 6;
    public const int TestMember7 = 7;
    public const int TestMember8 = 8;

    public const int TotalComponents = 9;

    public static readonly string[] componentNames = 
    {
        "TestFlag",
        "TestMember",
        "TestMember2",
        "TestMember3",
        "TestMember4",
        "TestMember5",
        "TestMember6",
        "TestMember7",
        "TestMember8"
    };

    public static readonly System.Type[] componentTypes = 
    {
        typeof(TestFlagComponent),
        typeof(TestMemberComponent),
        typeof(TestMember2Component),
        typeof(TestMember3Component),
        typeof(TestMember4Component),
        typeof(TestMember5Component),
        typeof(TestMember6Component),
        typeof(TestMember7Component),
        typeof(TestMember8Component)
    };
}


// BlablaContext.g.cs
public sealed partial class BlablaContext : Entitas.Context<BlablaEntity>
{
    public BlablaContext()
        : base(
            BlablaComponentsLookup.TotalComponents,
            0,
            new Entitas.ContextInfo(
                "Blabla",
                BlablaComponentsLookup.componentNames,
                BlablaComponentsLookup.componentTypes
            ),
            (entity) =>

#if (ENTITAS_FAST_AND_UNSAFE)
                new Entitas.UnsafeAERC(),
#else
                new Entitas.SafeAERC(entity),
#endif
            () => new BlablaEntity()
        ) 
    {
    }
}


// BlablaEntity.g.cs
public sealed partial class BlablaEntity : Entitas.Entity
{
}


// BlablaMatcher.g.cs
public sealed partial class BlablaMatcher 
{
    public static Entitas.IAllOfMatcher<BlablaEntity> AllOf(params int[] indices) 
    {
        return Entitas.Matcher<BlablaEntity>.AllOf(indices);
    }

    public static Entitas.IAllOfMatcher<BlablaEntity> AllOf(params Entitas.IMatcher<BlablaEntity>[] matchers)
    {
        return Entitas.Matcher<BlablaEntity>.AllOf(matchers);
    }

    public static Entitas.IAnyOfMatcher<BlablaEntity> AnyOf(params int[] indices)
    {
        return Entitas.Matcher<BlablaEntity>.AnyOf(indices);
    }

    public static Entitas.IAnyOfMatcher<BlablaEntity> AnyOf(params Entitas.IMatcher<BlablaEntity>[] matchers)
    {
        return Entitas.Matcher<BlablaEntity>.AnyOf(matchers);
    }
}


// BlablaTestFlagComponent.g.cs
public partial class BlablaEntity
{
    static readonly TestFlagComponent testFlagComponent = new TestFlagComponent();

    public bool isTestFlag
    {
        get { return HasComponent(BlablaComponentsLookup.TestFlag); }
        set 
        {
            if (value != isTestFlag)
            {
                var index = BlablaComponentsLookup.TestFlag;
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

public sealed partial class BlablaMatcher
{
    static Entitas.IMatcher<BlablaEntity> _matcherTestFlag;

    public static Entitas.IMatcher<BlablaEntity> TestFlag
    {
        get
        {
            if (_matcherTestFlag == null)
            {
                var matcher = (Entitas.Matcher<BlablaEntity>)Entitas.Matcher<BlablaEntity>.AllOf(BlablaComponentsLookup.TestFlag);
                matcher.componentNames = BlablaComponentsLookup.componentNames;
                _matcherTestFlag = matcher;
            }

            return _matcherTestFlag;
        }
    }
}


// BlablaTestMember2Component.g.cs
public partial class BlablaEntity
{
    public TestMember2Component testMember2 { get { return (TestMember2Component)GetComponent(BlablaComponentsLookup.TestMember2); } }
    public bool hasTestMember2 { get { return HasComponent(BlablaComponentsLookup.TestMember2); } }

    public void AddTestMember2(string newValue)
    {
        var index = BlablaComponentsLookup.TestMember2;
        var component = (TestMember2Component)CreateComponent(index, typeof(TestMember2Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember2(string newValue)
    {
        var index = BlablaComponentsLookup.TestMember2;
        var component = (TestMember2Component)CreateComponent(index, typeof(TestMember2Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember2()
    {
        RemoveComponent(BlablaComponentsLookup.TestMember2);
    }
}

public sealed partial class BlablaMatcher
{
    static Entitas.IMatcher<BlablaEntity> _matcherTestMember2;

    public static Entitas.IMatcher<BlablaEntity> TestMember2
    {
        get
        {
            if (_matcherTestMember2 == null)
            {
                var matcher = (Entitas.Matcher<BlablaEntity>)Entitas.Matcher<BlablaEntity>.AllOf(BlablaComponentsLookup.TestMember2);
                matcher.componentNames = BlablaComponentsLookup.componentNames;
                _matcherTestMember2 = matcher;
            }

            return _matcherTestMember2;
        }
    }
}


// BlablaTestMember3Component.g.cs
public partial class BlablaEntity
{
    public TestMember3Component testMember3 { get { return (TestMember3Component)GetComponent(BlablaComponentsLookup.TestMember3); } }
    public bool hasTestMember3 { get { return HasComponent(BlablaComponentsLookup.TestMember3); } }

    public void AddTestMember3(string newValue)
    {
        var index = BlablaComponentsLookup.TestMember3;
        var component = (TestMember3Component)CreateComponent(index, typeof(TestMember3Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember3(string newValue)
    {
        var index = BlablaComponentsLookup.TestMember3;
        var component = (TestMember3Component)CreateComponent(index, typeof(TestMember3Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember3()
    {
        RemoveComponent(BlablaComponentsLookup.TestMember3);
    }
}

public sealed partial class BlablaMatcher
{
    static Entitas.IMatcher<BlablaEntity> _matcherTestMember3;

    public static Entitas.IMatcher<BlablaEntity> TestMember3
    {
        get
        {
            if (_matcherTestMember3 == null)
            {
                var matcher = (Entitas.Matcher<BlablaEntity>)Entitas.Matcher<BlablaEntity>.AllOf(BlablaComponentsLookup.TestMember3);
                matcher.componentNames = BlablaComponentsLookup.componentNames;
                _matcherTestMember3 = matcher;
            }

            return _matcherTestMember3;
        }
    }
}


// BlablaTestMember4Component.g.cs
public partial class BlablaEntity
{
    public TestMember4Component testMember4 { get { return (TestMember4Component)GetComponent(BlablaComponentsLookup.TestMember4); } }
    public bool hasTestMember4 { get { return HasComponent(BlablaComponentsLookup.TestMember4); } }

    public void AddTestMember4(string newValue)
    {
        var index = BlablaComponentsLookup.TestMember4;
        var component = (TestMember4Component)CreateComponent(index, typeof(TestMember4Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember4(string newValue)
    {
        var index = BlablaComponentsLookup.TestMember4;
        var component = (TestMember4Component)CreateComponent(index, typeof(TestMember4Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember4()
    {
        RemoveComponent(BlablaComponentsLookup.TestMember4);
    }
}

public sealed partial class BlablaMatcher
{
    static Entitas.IMatcher<BlablaEntity> _matcherTestMember4;

    public static Entitas.IMatcher<BlablaEntity> TestMember4
    {
        get
        {
            if (_matcherTestMember4 == null)
            {
                var matcher = (Entitas.Matcher<BlablaEntity>)Entitas.Matcher<BlablaEntity>.AllOf(BlablaComponentsLookup.TestMember4);
                matcher.componentNames = BlablaComponentsLookup.componentNames;
                _matcherTestMember4 = matcher;
            }

            return _matcherTestMember4;
        }
    }
}


// BlablaTestMember5Component.g.cs
public partial class BlablaEntity
{
    public TestMember5Component testMember5 { get { return (TestMember5Component)GetComponent(BlablaComponentsLookup.TestMember5); } }
    public bool hasTestMember5 { get { return HasComponent(BlablaComponentsLookup.TestMember5); } }

    public void AddTestMember5(string newValue)
    {
        var index = BlablaComponentsLookup.TestMember5;
        var component = (TestMember5Component)CreateComponent(index, typeof(TestMember5Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember5(string newValue)
    {
        var index = BlablaComponentsLookup.TestMember5;
        var component = (TestMember5Component)CreateComponent(index, typeof(TestMember5Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember5()
    {
        RemoveComponent(BlablaComponentsLookup.TestMember5);
    }
}

public sealed partial class BlablaMatcher
{
    static Entitas.IMatcher<BlablaEntity> _matcherTestMember5;

    public static Entitas.IMatcher<BlablaEntity> TestMember5
    {
        get
        {
            if (_matcherTestMember5 == null)
            {
                var matcher = (Entitas.Matcher<BlablaEntity>)Entitas.Matcher<BlablaEntity>.AllOf(BlablaComponentsLookup.TestMember5);
                matcher.componentNames = BlablaComponentsLookup.componentNames;
                _matcherTestMember5 = matcher;
            }

            return _matcherTestMember5;
        }
    }
}


// BlablaTestMember6Component.g.cs
public partial class BlablaEntity
{
    public TestMember6Component testMember6 { get { return (TestMember6Component)GetComponent(BlablaComponentsLookup.TestMember6); } }
    public bool hasTestMember6 { get { return HasComponent(BlablaComponentsLookup.TestMember6); } }

    public void AddTestMember6(string newValue)
    {
        var index = BlablaComponentsLookup.TestMember6;
        var component = (TestMember6Component)CreateComponent(index, typeof(TestMember6Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember6(string newValue)
    {
        var index = BlablaComponentsLookup.TestMember6;
        var component = (TestMember6Component)CreateComponent(index, typeof(TestMember6Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember6()
    {
        RemoveComponent(BlablaComponentsLookup.TestMember6);
    }
}

public sealed partial class BlablaMatcher
{
    static Entitas.IMatcher<BlablaEntity> _matcherTestMember6;

    public static Entitas.IMatcher<BlablaEntity> TestMember6
    {
        get
        {
            if (_matcherTestMember6 == null)
            {
                var matcher = (Entitas.Matcher<BlablaEntity>)Entitas.Matcher<BlablaEntity>.AllOf(BlablaComponentsLookup.TestMember6);
                matcher.componentNames = BlablaComponentsLookup.componentNames;
                _matcherTestMember6 = matcher;
            }

            return _matcherTestMember6;
        }
    }
}


// BlablaTestMember7Component.g.cs
public partial class BlablaEntity
{
    public TestMember7Component testMember7 { get { return (TestMember7Component)GetComponent(BlablaComponentsLookup.TestMember7); } }
    public bool hasTestMember7 { get { return HasComponent(BlablaComponentsLookup.TestMember7); } }

    public void AddTestMember7(string newValue)
    {
        var index = BlablaComponentsLookup.TestMember7;
        var component = (TestMember7Component)CreateComponent(index, typeof(TestMember7Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember7(string newValue)
    {
        var index = BlablaComponentsLookup.TestMember7;
        var component = (TestMember7Component)CreateComponent(index, typeof(TestMember7Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember7()
    {
        RemoveComponent(BlablaComponentsLookup.TestMember7);
    }
}

public sealed partial class BlablaMatcher
{
    static Entitas.IMatcher<BlablaEntity> _matcherTestMember7;

    public static Entitas.IMatcher<BlablaEntity> TestMember7
    {
        get
        {
            if (_matcherTestMember7 == null)
            {
                var matcher = (Entitas.Matcher<BlablaEntity>)Entitas.Matcher<BlablaEntity>.AllOf(BlablaComponentsLookup.TestMember7);
                matcher.componentNames = BlablaComponentsLookup.componentNames;
                _matcherTestMember7 = matcher;
            }

            return _matcherTestMember7;
        }
    }
}


// BlablaTestMember8Component.g.cs
public partial class BlablaEntity
{
    public TestMember8Component testMember8 { get { return (TestMember8Component)GetComponent(BlablaComponentsLookup.TestMember8); } }
    public bool hasTestMember8 { get { return HasComponent(BlablaComponentsLookup.TestMember8); } }

    public void AddTestMember8(string newValue)
    {
        var index = BlablaComponentsLookup.TestMember8;
        var component = (TestMember8Component)CreateComponent(index, typeof(TestMember8Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember8(string newValue)
    {
        var index = BlablaComponentsLookup.TestMember8;
        var component = (TestMember8Component)CreateComponent(index, typeof(TestMember8Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember8()
    {
        RemoveComponent(BlablaComponentsLookup.TestMember8);
    }
}

public sealed partial class BlablaMatcher
{
    static Entitas.IMatcher<BlablaEntity> _matcherTestMember8;

    public static Entitas.IMatcher<BlablaEntity> TestMember8
    {
        get
        {
            if (_matcherTestMember8 == null)
            {
                var matcher = (Entitas.Matcher<BlablaEntity>)Entitas.Matcher<BlablaEntity>.AllOf(BlablaComponentsLookup.TestMember8);
                matcher.componentNames = BlablaComponentsLookup.componentNames;
                _matcherTestMember8 = matcher;
            }

            return _matcherTestMember8;
        }
    }
}


// BlablaTestMemberComponent.g.cs
public partial class BlablaEntity
{
    public TestMemberComponent testMember { get { return (TestMemberComponent)GetComponent(BlablaComponentsLookup.TestMember); } }
    public bool hasTestMember { get { return HasComponent(BlablaComponentsLookup.TestMember); } }

    public void AddTestMember(string newValue)
    {
        var index = BlablaComponentsLookup.TestMember;
        var component = (TestMemberComponent)CreateComponent(index, typeof(TestMemberComponent));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember(string newValue)
    {
        var index = BlablaComponentsLookup.TestMember;
        var component = (TestMemberComponent)CreateComponent(index, typeof(TestMemberComponent));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember()
    {
        RemoveComponent(BlablaComponentsLookup.TestMember);
    }
}

public sealed partial class BlablaMatcher
{
    static Entitas.IMatcher<BlablaEntity> _matcherTestMember;

    public static Entitas.IMatcher<BlablaEntity> TestMember
    {
        get
        {
            if (_matcherTestMember == null)
            {
                var matcher = (Entitas.Matcher<BlablaEntity>)Entitas.Matcher<BlablaEntity>.AllOf(BlablaComponentsLookup.TestMember);
                matcher.componentNames = BlablaComponentsLookup.componentNames;
                _matcherTestMember = matcher;
            }

            return _matcherTestMember;
        }
    }
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

    public InputContext Input { get; set; }
    public GameContext Game { get; set; }
    public BlablaContext Blabla { get; set; }

    public Entitas.IContext[] allContexts { get { return new Entitas.IContext [] { Input, Game, Blabla }; } }

    public Contexts()
    {
        Input = new InputContext();
        Game = new GameContext();
        Blabla = new BlablaContext();

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


// GameComponentsLookup.g.cs
public static class GameComponentsLookup
{
    public const int TestFlag = 0;
    public const int TestMember = 1;
    public const int TestMember2 = 2;
    public const int TestMember3 = 3;
    public const int TestMember4 = 4;
    public const int TestMember5 = 5;
    public const int TestMember6 = 6;
    public const int TestMember7 = 7;
    public const int TestMember8 = 8;

    public const int TotalComponents = 9;

    public static readonly string[] componentNames = 
    {
        "TestFlag",
        "TestMember",
        "TestMember2",
        "TestMember3",
        "TestMember4",
        "TestMember5",
        "TestMember6",
        "TestMember7",
        "TestMember8"
    };

    public static readonly System.Type[] componentTypes = 
    {
        typeof(TestFlagComponent),
        typeof(TestMemberComponent),
        typeof(TestMember2Component),
        typeof(TestMember3Component),
        typeof(TestMember4Component),
        typeof(TestMember5Component),
        typeof(TestMember6Component),
        typeof(TestMember7Component),
        typeof(TestMember8Component)
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


// GameTestMember3Component.g.cs
public partial class GameEntity
{
    public TestMember3Component testMember3 { get { return (TestMember3Component)GetComponent(GameComponentsLookup.TestMember3); } }
    public bool hasTestMember3 { get { return HasComponent(GameComponentsLookup.TestMember3); } }

    public void AddTestMember3(string newValue)
    {
        var index = GameComponentsLookup.TestMember3;
        var component = (TestMember3Component)CreateComponent(index, typeof(TestMember3Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember3(string newValue)
    {
        var index = GameComponentsLookup.TestMember3;
        var component = (TestMember3Component)CreateComponent(index, typeof(TestMember3Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember3()
    {
        RemoveComponent(GameComponentsLookup.TestMember3);
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherTestMember3;

    public static Entitas.IMatcher<GameEntity> TestMember3
    {
        get
        {
            if (_matcherTestMember3 == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.TestMember3);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherTestMember3 = matcher;
            }

            return _matcherTestMember3;
        }
    }
}


// GameTestMember4Component.g.cs
public partial class GameEntity
{
    public TestMember4Component testMember4 { get { return (TestMember4Component)GetComponent(GameComponentsLookup.TestMember4); } }
    public bool hasTestMember4 { get { return HasComponent(GameComponentsLookup.TestMember4); } }

    public void AddTestMember4(string newValue)
    {
        var index = GameComponentsLookup.TestMember4;
        var component = (TestMember4Component)CreateComponent(index, typeof(TestMember4Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember4(string newValue)
    {
        var index = GameComponentsLookup.TestMember4;
        var component = (TestMember4Component)CreateComponent(index, typeof(TestMember4Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember4()
    {
        RemoveComponent(GameComponentsLookup.TestMember4);
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherTestMember4;

    public static Entitas.IMatcher<GameEntity> TestMember4
    {
        get
        {
            if (_matcherTestMember4 == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.TestMember4);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherTestMember4 = matcher;
            }

            return _matcherTestMember4;
        }
    }
}


// GameTestMember5Component.g.cs
public partial class GameEntity
{
    public TestMember5Component testMember5 { get { return (TestMember5Component)GetComponent(GameComponentsLookup.TestMember5); } }
    public bool hasTestMember5 { get { return HasComponent(GameComponentsLookup.TestMember5); } }

    public void AddTestMember5(string newValue)
    {
        var index = GameComponentsLookup.TestMember5;
        var component = (TestMember5Component)CreateComponent(index, typeof(TestMember5Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember5(string newValue)
    {
        var index = GameComponentsLookup.TestMember5;
        var component = (TestMember5Component)CreateComponent(index, typeof(TestMember5Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember5()
    {
        RemoveComponent(GameComponentsLookup.TestMember5);
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherTestMember5;

    public static Entitas.IMatcher<GameEntity> TestMember5
    {
        get
        {
            if (_matcherTestMember5 == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.TestMember5);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherTestMember5 = matcher;
            }

            return _matcherTestMember5;
        }
    }
}


// GameTestMember6Component.g.cs
public partial class GameEntity
{
    public TestMember6Component testMember6 { get { return (TestMember6Component)GetComponent(GameComponentsLookup.TestMember6); } }
    public bool hasTestMember6 { get { return HasComponent(GameComponentsLookup.TestMember6); } }

    public void AddTestMember6(string newValue)
    {
        var index = GameComponentsLookup.TestMember6;
        var component = (TestMember6Component)CreateComponent(index, typeof(TestMember6Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember6(string newValue)
    {
        var index = GameComponentsLookup.TestMember6;
        var component = (TestMember6Component)CreateComponent(index, typeof(TestMember6Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember6()
    {
        RemoveComponent(GameComponentsLookup.TestMember6);
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherTestMember6;

    public static Entitas.IMatcher<GameEntity> TestMember6
    {
        get
        {
            if (_matcherTestMember6 == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.TestMember6);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherTestMember6 = matcher;
            }

            return _matcherTestMember6;
        }
    }
}


// GameTestMember7Component.g.cs
public partial class GameEntity
{
    public TestMember7Component testMember7 { get { return (TestMember7Component)GetComponent(GameComponentsLookup.TestMember7); } }
    public bool hasTestMember7 { get { return HasComponent(GameComponentsLookup.TestMember7); } }

    public void AddTestMember7(string newValue)
    {
        var index = GameComponentsLookup.TestMember7;
        var component = (TestMember7Component)CreateComponent(index, typeof(TestMember7Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember7(string newValue)
    {
        var index = GameComponentsLookup.TestMember7;
        var component = (TestMember7Component)CreateComponent(index, typeof(TestMember7Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember7()
    {
        RemoveComponent(GameComponentsLookup.TestMember7);
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherTestMember7;

    public static Entitas.IMatcher<GameEntity> TestMember7
    {
        get
        {
            if (_matcherTestMember7 == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.TestMember7);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherTestMember7 = matcher;
            }

            return _matcherTestMember7;
        }
    }
}


// GameTestMember8Component.g.cs
public partial class GameEntity
{
    public TestMember8Component testMember8 { get { return (TestMember8Component)GetComponent(GameComponentsLookup.TestMember8); } }
    public bool hasTestMember8 { get { return HasComponent(GameComponentsLookup.TestMember8); } }

    public void AddTestMember8(string newValue)
    {
        var index = GameComponentsLookup.TestMember8;
        var component = (TestMember8Component)CreateComponent(index, typeof(TestMember8Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember8(string newValue)
    {
        var index = GameComponentsLookup.TestMember8;
        var component = (TestMember8Component)CreateComponent(index, typeof(TestMember8Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember8()
    {
        RemoveComponent(GameComponentsLookup.TestMember8);
    }
}

public sealed partial class GameMatcher
{
    static Entitas.IMatcher<GameEntity> _matcherTestMember8;

    public static Entitas.IMatcher<GameEntity> TestMember8
    {
        get
        {
            if (_matcherTestMember8 == null)
            {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.TestMember8);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherTestMember8 = matcher;
            }

            return _matcherTestMember8;
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


// InputComponentsLookup.g.cs
public static class InputComponentsLookup
{
    public const int TestFlag = 0;
    public const int TestMember = 1;
    public const int TestMember2 = 2;
    public const int TestMember3 = 3;
    public const int TestMember4 = 4;
    public const int TestMember5 = 5;
    public const int TestMember6 = 6;
    public const int TestMember7 = 7;
    public const int TestMember8 = 8;

    public const int TotalComponents = 9;

    public static readonly string[] componentNames = 
    {
        "TestFlag",
        "TestMember",
        "TestMember2",
        "TestMember3",
        "TestMember4",
        "TestMember5",
        "TestMember6",
        "TestMember7",
        "TestMember8"
    };

    public static readonly System.Type[] componentTypes = 
    {
        typeof(TestFlagComponent),
        typeof(TestMemberComponent),
        typeof(TestMember2Component),
        typeof(TestMember3Component),
        typeof(TestMember4Component),
        typeof(TestMember5Component),
        typeof(TestMember6Component),
        typeof(TestMember7Component),
        typeof(TestMember8Component)
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


// InputTestMember2Component.g.cs
public partial class InputEntity
{
    public TestMember2Component testMember2 { get { return (TestMember2Component)GetComponent(InputComponentsLookup.TestMember2); } }
    public bool hasTestMember2 { get { return HasComponent(InputComponentsLookup.TestMember2); } }

    public void AddTestMember2(string newValue)
    {
        var index = InputComponentsLookup.TestMember2;
        var component = (TestMember2Component)CreateComponent(index, typeof(TestMember2Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember2(string newValue)
    {
        var index = InputComponentsLookup.TestMember2;
        var component = (TestMember2Component)CreateComponent(index, typeof(TestMember2Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember2()
    {
        RemoveComponent(InputComponentsLookup.TestMember2);
    }
}

public sealed partial class InputMatcher
{
    static Entitas.IMatcher<InputEntity> _matcherTestMember2;

    public static Entitas.IMatcher<InputEntity> TestMember2
    {
        get
        {
            if (_matcherTestMember2 == null)
            {
                var matcher = (Entitas.Matcher<InputEntity>)Entitas.Matcher<InputEntity>.AllOf(InputComponentsLookup.TestMember2);
                matcher.componentNames = InputComponentsLookup.componentNames;
                _matcherTestMember2 = matcher;
            }

            return _matcherTestMember2;
        }
    }
}


// InputTestMember3Component.g.cs
public partial class InputEntity
{
    public TestMember3Component testMember3 { get { return (TestMember3Component)GetComponent(InputComponentsLookup.TestMember3); } }
    public bool hasTestMember3 { get { return HasComponent(InputComponentsLookup.TestMember3); } }

    public void AddTestMember3(string newValue)
    {
        var index = InputComponentsLookup.TestMember3;
        var component = (TestMember3Component)CreateComponent(index, typeof(TestMember3Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember3(string newValue)
    {
        var index = InputComponentsLookup.TestMember3;
        var component = (TestMember3Component)CreateComponent(index, typeof(TestMember3Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember3()
    {
        RemoveComponent(InputComponentsLookup.TestMember3);
    }
}

public sealed partial class InputMatcher
{
    static Entitas.IMatcher<InputEntity> _matcherTestMember3;

    public static Entitas.IMatcher<InputEntity> TestMember3
    {
        get
        {
            if (_matcherTestMember3 == null)
            {
                var matcher = (Entitas.Matcher<InputEntity>)Entitas.Matcher<InputEntity>.AllOf(InputComponentsLookup.TestMember3);
                matcher.componentNames = InputComponentsLookup.componentNames;
                _matcherTestMember3 = matcher;
            }

            return _matcherTestMember3;
        }
    }
}


// InputTestMember4Component.g.cs
public partial class InputEntity
{
    public TestMember4Component testMember4 { get { return (TestMember4Component)GetComponent(InputComponentsLookup.TestMember4); } }
    public bool hasTestMember4 { get { return HasComponent(InputComponentsLookup.TestMember4); } }

    public void AddTestMember4(string newValue)
    {
        var index = InputComponentsLookup.TestMember4;
        var component = (TestMember4Component)CreateComponent(index, typeof(TestMember4Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember4(string newValue)
    {
        var index = InputComponentsLookup.TestMember4;
        var component = (TestMember4Component)CreateComponent(index, typeof(TestMember4Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember4()
    {
        RemoveComponent(InputComponentsLookup.TestMember4);
    }
}

public sealed partial class InputMatcher
{
    static Entitas.IMatcher<InputEntity> _matcherTestMember4;

    public static Entitas.IMatcher<InputEntity> TestMember4
    {
        get
        {
            if (_matcherTestMember4 == null)
            {
                var matcher = (Entitas.Matcher<InputEntity>)Entitas.Matcher<InputEntity>.AllOf(InputComponentsLookup.TestMember4);
                matcher.componentNames = InputComponentsLookup.componentNames;
                _matcherTestMember4 = matcher;
            }

            return _matcherTestMember4;
        }
    }
}


// InputTestMember5Component.g.cs
public partial class InputEntity
{
    public TestMember5Component testMember5 { get { return (TestMember5Component)GetComponent(InputComponentsLookup.TestMember5); } }
    public bool hasTestMember5 { get { return HasComponent(InputComponentsLookup.TestMember5); } }

    public void AddTestMember5(string newValue)
    {
        var index = InputComponentsLookup.TestMember5;
        var component = (TestMember5Component)CreateComponent(index, typeof(TestMember5Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember5(string newValue)
    {
        var index = InputComponentsLookup.TestMember5;
        var component = (TestMember5Component)CreateComponent(index, typeof(TestMember5Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember5()
    {
        RemoveComponent(InputComponentsLookup.TestMember5);
    }
}

public sealed partial class InputMatcher
{
    static Entitas.IMatcher<InputEntity> _matcherTestMember5;

    public static Entitas.IMatcher<InputEntity> TestMember5
    {
        get
        {
            if (_matcherTestMember5 == null)
            {
                var matcher = (Entitas.Matcher<InputEntity>)Entitas.Matcher<InputEntity>.AllOf(InputComponentsLookup.TestMember5);
                matcher.componentNames = InputComponentsLookup.componentNames;
                _matcherTestMember5 = matcher;
            }

            return _matcherTestMember5;
        }
    }
}


// InputTestMember6Component.g.cs
public partial class InputEntity
{
    public TestMember6Component testMember6 { get { return (TestMember6Component)GetComponent(InputComponentsLookup.TestMember6); } }
    public bool hasTestMember6 { get { return HasComponent(InputComponentsLookup.TestMember6); } }

    public void AddTestMember6(string newValue)
    {
        var index = InputComponentsLookup.TestMember6;
        var component = (TestMember6Component)CreateComponent(index, typeof(TestMember6Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember6(string newValue)
    {
        var index = InputComponentsLookup.TestMember6;
        var component = (TestMember6Component)CreateComponent(index, typeof(TestMember6Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember6()
    {
        RemoveComponent(InputComponentsLookup.TestMember6);
    }
}

public sealed partial class InputMatcher
{
    static Entitas.IMatcher<InputEntity> _matcherTestMember6;

    public static Entitas.IMatcher<InputEntity> TestMember6
    {
        get
        {
            if (_matcherTestMember6 == null)
            {
                var matcher = (Entitas.Matcher<InputEntity>)Entitas.Matcher<InputEntity>.AllOf(InputComponentsLookup.TestMember6);
                matcher.componentNames = InputComponentsLookup.componentNames;
                _matcherTestMember6 = matcher;
            }

            return _matcherTestMember6;
        }
    }
}


// InputTestMember7Component.g.cs
public partial class InputEntity
{
    public TestMember7Component testMember7 { get { return (TestMember7Component)GetComponent(InputComponentsLookup.TestMember7); } }
    public bool hasTestMember7 { get { return HasComponent(InputComponentsLookup.TestMember7); } }

    public void AddTestMember7(string newValue)
    {
        var index = InputComponentsLookup.TestMember7;
        var component = (TestMember7Component)CreateComponent(index, typeof(TestMember7Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember7(string newValue)
    {
        var index = InputComponentsLookup.TestMember7;
        var component = (TestMember7Component)CreateComponent(index, typeof(TestMember7Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember7()
    {
        RemoveComponent(InputComponentsLookup.TestMember7);
    }
}

public sealed partial class InputMatcher
{
    static Entitas.IMatcher<InputEntity> _matcherTestMember7;

    public static Entitas.IMatcher<InputEntity> TestMember7
    {
        get
        {
            if (_matcherTestMember7 == null)
            {
                var matcher = (Entitas.Matcher<InputEntity>)Entitas.Matcher<InputEntity>.AllOf(InputComponentsLookup.TestMember7);
                matcher.componentNames = InputComponentsLookup.componentNames;
                _matcherTestMember7 = matcher;
            }

            return _matcherTestMember7;
        }
    }
}


// InputTestMember8Component.g.cs
public partial class InputEntity
{
    public TestMember8Component testMember8 { get { return (TestMember8Component)GetComponent(InputComponentsLookup.TestMember8); } }
    public bool hasTestMember8 { get { return HasComponent(InputComponentsLookup.TestMember8); } }

    public void AddTestMember8(string newValue)
    {
        var index = InputComponentsLookup.TestMember8;
        var component = (TestMember8Component)CreateComponent(index, typeof(TestMember8Component));
        component.Value = newValue;
        AddComponent(index, component);
    }

    public void ReplaceTestMember8(string newValue)
    {
        var index = InputComponentsLookup.TestMember8;
        var component = (TestMember8Component)CreateComponent(index, typeof(TestMember8Component));
        component.Value = newValue;
        ReplaceComponent(index, component);
    }

    public void RemoveTestMember8()
    {
        RemoveComponent(InputComponentsLookup.TestMember8);
    }
}

public sealed partial class InputMatcher
{
    static Entitas.IMatcher<InputEntity> _matcherTestMember8;

    public static Entitas.IMatcher<InputEntity> TestMember8
    {
        get
        {
            if (_matcherTestMember8 == null)
            {
                var matcher = (Entitas.Matcher<InputEntity>)Entitas.Matcher<InputEntity>.AllOf(InputComponentsLookup.TestMember8);
                matcher.componentNames = InputComponentsLookup.componentNames;
                _matcherTestMember8 = matcher;
            }

            return _matcherTestMember8;
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

public partial class BlablaEntity : ITestFlagEntity { }

public partial class InputEntity : ITestFlagEntity { }


// ITestMember2Entity.g.cs
public partial interface ITestMember2Entity
{
    TestMember2Component testMember2 { get; }
    bool hasTestMember2 { get; }

    void AddTestMember2(string newValue);
    void ReplaceTestMember2(string newValue);
    void RemoveTestMember2();
}

public partial class GameEntity : ITestMember2Entity { }

public partial class BlablaEntity : ITestMember2Entity { }

public partial class InputEntity : ITestMember2Entity { }


// ITestMember3Entity.g.cs
public partial interface ITestMember3Entity
{
    TestMember3Component testMember3 { get; }
    bool hasTestMember3 { get; }

    void AddTestMember3(string newValue);
    void ReplaceTestMember3(string newValue);
    void RemoveTestMember3();
}

public partial class GameEntity : ITestMember3Entity { }

public partial class BlablaEntity : ITestMember3Entity { }

public partial class InputEntity : ITestMember3Entity { }


// ITestMember4Entity.g.cs
public partial interface ITestMember4Entity
{
    TestMember4Component testMember4 { get; }
    bool hasTestMember4 { get; }

    void AddTestMember4(string newValue);
    void ReplaceTestMember4(string newValue);
    void RemoveTestMember4();
}

public partial class GameEntity : ITestMember4Entity { }

public partial class BlablaEntity : ITestMember4Entity { }

public partial class InputEntity : ITestMember4Entity { }


// ITestMember5Entity.g.cs
public partial interface ITestMember5Entity
{
    TestMember5Component testMember5 { get; }
    bool hasTestMember5 { get; }

    void AddTestMember5(string newValue);
    void ReplaceTestMember5(string newValue);
    void RemoveTestMember5();
}

public partial class GameEntity : ITestMember5Entity { }

public partial class BlablaEntity : ITestMember5Entity { }

public partial class InputEntity : ITestMember5Entity { }


// ITestMember6Entity.g.cs
public partial interface ITestMember6Entity
{
    TestMember6Component testMember6 { get; }
    bool hasTestMember6 { get; }

    void AddTestMember6(string newValue);
    void ReplaceTestMember6(string newValue);
    void RemoveTestMember6();
}

public partial class GameEntity : ITestMember6Entity { }

public partial class BlablaEntity : ITestMember6Entity { }

public partial class InputEntity : ITestMember6Entity { }


// ITestMember7Entity.g.cs
public partial interface ITestMember7Entity
{
    TestMember7Component testMember7 { get; }
    bool hasTestMember7 { get; }

    void AddTestMember7(string newValue);
    void ReplaceTestMember7(string newValue);
    void RemoveTestMember7();
}

public partial class GameEntity : ITestMember7Entity { }

public partial class BlablaEntity : ITestMember7Entity { }

public partial class InputEntity : ITestMember7Entity { }


// ITestMember8Entity.g.cs
public partial interface ITestMember8Entity
{
    TestMember8Component testMember8 { get; }
    bool hasTestMember8 { get; }

    void AddTestMember8(string newValue);
    void ReplaceTestMember8(string newValue);
    void RemoveTestMember8();
}

public partial class GameEntity : ITestMember8Entity { }

public partial class BlablaEntity : ITestMember8Entity { }

public partial class InputEntity : ITestMember8Entity { }


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

public partial class BlablaEntity : ITestMemberEntity { }

public partial class InputEntity : ITestMemberEntity { }
