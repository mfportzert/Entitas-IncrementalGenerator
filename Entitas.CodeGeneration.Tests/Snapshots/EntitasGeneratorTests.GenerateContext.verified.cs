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


// GameComponentsLookup.g.cs
public static class GameComponentsLookup
{


    public const int TotalComponents = 0;

    public static readonly string[] componentNames = 
    {

    };

    public static readonly System.Type[] componentTypes = 
    {

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


// InputComponentsLookup.g.cs
public static class InputComponentsLookup
{


    public const int TotalComponents = 0;

    public static readonly string[] componentNames = 
    {

    };

    public static readonly System.Type[] componentTypes = 
    {

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
