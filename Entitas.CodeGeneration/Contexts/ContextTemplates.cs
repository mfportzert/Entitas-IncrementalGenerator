namespace Entitas.CodeGeneration.Contexts;

public static class ContextTemplates
{
    public const string ContextsTemplate =
        @"public partial class Contexts : Entitas.IContexts
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

${contextPropertyList}

    public Entitas.IContext[] allContexts { get { return new Entitas.IContext [] { ${contextList} }; } }

    public Contexts()
    {
${contextAssignmentList}

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
";
    
    public const string ContextPropertyTemplate = @"    public ${ContextType} ${contextName} { get; set; }";
    public const string ContextListTemplate = @"${contextName}";
    public const string ContextAssignmentTemplate = @"        ${contextName} = new ${ContextType}();";
    
    public const string ContextTemplate =
        @"public sealed partial class ${ContextType} : Entitas.Context<${EntityType}>
{
    public ${ContextType}()
        : base(
            ${Lookup}.TotalComponents,
            0,
            new Entitas.ContextInfo(
                ""${ContextName}"",
                ${Lookup}.componentNames,
                ${Lookup}.componentTypes
            ),
            (entity) =>

#if (ENTITAS_FAST_AND_UNSAFE)
                new Entitas.UnsafeAERC(),
#else
                new Entitas.SafeAERC(entity),
#endif
            () => new ${EntityType}()
        ) 
    {
    }
}
";
    
    public const string ContextMatcherTemplate =
        @"public sealed partial class ${MatcherType} 
{
    public static Entitas.IAllOfMatcher<${EntityType}> AllOf(params int[] indices) 
    {
        return Entitas.Matcher<${EntityType}>.AllOf(indices);
    }

    public static Entitas.IAllOfMatcher<${EntityType}> AllOf(params Entitas.IMatcher<${EntityType}>[] matchers)
    {
        return Entitas.Matcher<${EntityType}>.AllOf(matchers);
    }

    public static Entitas.IAnyOfMatcher<${EntityType}> AnyOf(params int[] indices)
    {
        return Entitas.Matcher<${EntityType}>.AnyOf(indices);
    }

    public static Entitas.IAnyOfMatcher<${EntityType}> AnyOf(params Entitas.IMatcher<${EntityType}>[] matchers)
    {
        return Entitas.Matcher<${EntityType}>.AnyOf(matchers);
    }
}
";
    
    public const string ContextEntityTemplate =
        @"public sealed partial class ${EntityType} : Entitas.Entity
{
}
";
}