using Entitas.CodeGeneration.Components.Data;
using Entitas.CodeGeneration.Components.Extensions;
using Entitas.CodeGeneration.Contexts.Data;
using Entitas.CodeGeneration.Extensions;

namespace Entitas.CodeGeneration.Components;

public static class ComponentTemplates
{
    public const string GeneratedComponentTemplate =
        @"[Entitas.CodeGeneration.Attributes.DontGenerate(false)]
public sealed class ${FullComponentName} : Entitas.IComponent
{
    public ${Type} value;
}
";

    const string StandardComponentContextApiTemplate =
        @"public partial class ${ContextType}
{
    public ${EntityType} ${componentName}Entity { get { return GetGroup(${MatcherType}.${ComponentName}).GetSingleEntity(); } }
    public ${ComponentType} ${validComponentName} { get { return ${componentName}Entity.${componentName}; } }
    public bool has${ComponentName} { get { return ${componentName}Entity != null; } }

    public ${EntityType} Set${ComponentName}(${newMethodParameters})
    {
        if (has${ComponentName})
        {
            throw new Entitas.EntitasException(""Could not set ${ComponentName}!\n"" + this + "" already has an entity with ${ComponentType}!"",
                ""You should check if the context already has a ${componentName}Entity before setting it or use context.Replace${ComponentName}()."");
        }
        var entity = CreateEntity();
        entity.Add${ComponentName}(${newMethodArgs});
        return entity;
    }

    public void Replace${ComponentName}(${newMethodParameters})
    {
        var entity = ${componentName}Entity;
        if (entity == null)
        {
            entity = Set${ComponentName}(${newMethodArgs});
        }
        else
        {
            entity.Replace${ComponentName}(${newMethodArgs});
        }
    }

    public void Remove${ComponentName}()
    {
        ${componentName}Entity.Destroy();
    }
}
";
    
    public static string GetStandardComponentContextApiSource(
        in ContextData contextData,
        in ComponentData componentData)
    {
        var componentName = componentData.GetComponentName();
        var componentNameLower = componentName.ToLowerFirst();
        var validComponentName = componentNameLower.AddPrefixIfIsKeyword();
        var newMethodParameters = componentData.Members.GetMethodParameters(true);
        var newMethodArgs = componentData.Members.GetMethodArgs(true);

        return StandardComponentContextApiTemplate
            .Replace("${ContextType}", contextData.ContextTypeName)
            .Replace("${EntityType}", contextData.EntityTypeName)
            .Replace("${ComponentName}", componentName)
            .Replace("${componentName}", componentNameLower)
            .Replace("${MatcherType}", contextData.MatcherTypeName)
            .Replace("${ComponentType}", componentData.FullTypeName)
            .Replace("${validComponentName}", validComponentName)
            .Replace("${newMethodParameters}", newMethodParameters)
            .Replace("${newMethodArgs}", newMethodArgs);
    }
    
    const string FlagComponentContextApiTemplate =
        @"public partial class ${ContextType}
{
    public ${EntityType} ${componentName}Entity { get { return GetGroup(${MatcherType}.${ComponentName}).GetSingleEntity(); } }

    public bool ${prefixedComponentName}
    {
        get { return ${componentName}Entity != null; }
        set
        {
            var entity = ${componentName}Entity;
            if (value != (entity != null))
            {
                if (value)
                {
                    CreateEntity().${prefixedComponentName} = true;
                }
                else
                {
                    entity.Destroy();
                }
            }
        }
    }
}
";

    // (unique) components without members
    public static string GetFlagComponentContextApiSource(
        in ContextData contextData,
        in ComponentData componentData)
    {
        var componentName = componentData.GetComponentName();
        var componentNameLower = componentName.ToLowerFirst();
        
        return FlagComponentContextApiTemplate
            .Replace("${ContextType}", contextData.ContextTypeName)
            .Replace("${EntityType}", contextData.EntityTypeName)
            .Replace("${ComponentName}", componentName)
            .Replace("${componentName}", componentNameLower)
            .Replace("${MatcherType}", contextData.MatcherTypeName)
            .Replace("${PrefixedComponentName}", componentData.PrefixedComponentName());
    }
    
    const string StandardComponentEntityApiTemplate =
        @"public partial class ${EntityType}
{
    public ${ComponentType} ${validComponentName} { get { return (${ComponentType})GetComponent(${Index}); } }
    public bool has${ComponentName} { get { return HasComponent(${Index}); } }

    public void Add${ComponentName}(${newMethodParameters})
    {
        var index = ${Index};
        var component = (${ComponentType})CreateComponent(index, typeof(${ComponentType}));
${memberAssignmentList}
        AddComponent(index, component);
    }

    public void Replace${ComponentName}(${newMethodParameters})
    {
        var index = ${Index};
        var component = (${ComponentType})CreateComponent(index, typeof(${ComponentType}));
${memberAssignmentList}
        ReplaceComponent(index, component);
    }

    public void Remove${ComponentName}()
    {
        RemoveComponent(${Index});
    }
}
";

    public static string GetStandardComponentEntityApiSource(
        in ContextData contextData,
        in ComponentData componentData)
    {
        var componentName = componentData.GetComponentName();
        var componentIndex = componentData.GetComponentIndex(contextData);
        var validComponentName = componentName.ToLowerFirst().AddPrefixIfIsKeyword();
        var newMethodParameters = componentData.Members.GetMethodParameters(true);
        var memberAssignmentList = componentData.Members.GetMemberAssignmentList();

        return StandardComponentEntityApiTemplate
            .Replace("${EntityType}", contextData.EntityTypeName)
            .Replace("${ComponentType}", componentData.FullTypeName)
            .Replace("${ComponentName}", componentName)
            .Replace("${Index}", componentIndex)
            .Replace("${validComponentName}", validComponentName)
            .Replace("${newMethodParameters}", newMethodParameters)
            .Replace("${memberAssignmentList}", memberAssignmentList);
    }

    const string FlagComponentEntityApiTemplate =
        @"public partial class ${EntityType}
{
    static readonly ${ComponentType} ${componentName}Component = new ${ComponentType}();

    public bool ${prefixedComponentName}
    {
        get { return HasComponent(${Index}); }
        set 
        {
            if (value != ${prefixedComponentName})
            {
                var index = ${Index};
                if (value)
                {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : ${componentName}Component;

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
";
    
    public static string GetFlagComponentEntityApiSource(
        in ContextData contextData,
        in ComponentData componentData)
    {
        return FlagComponentEntityApiTemplate
            .Replace("${EntityType}", contextData.EntityTypeName)
            .Replace("${ComponentType}", componentData.FullTypeName)
            .Replace("${componentName}", componentData.GetComponentNameLowerFirst())
            .Replace("${Index}", componentData.GetComponentIndex(contextData))
            .Replace("${prefixedComponentName}", componentData.PrefixedComponentName());
    }

    const string StandardComponentInterfaceTemplate =
        @"public partial interface I${ComponentName}Entity
{
    ${ComponentType} ${validComponentName} { get; }
    bool has${ComponentName} { get; }

    void Add${ComponentName}(${newMethodParameters});
    void Replace${ComponentName}(${newMethodParameters});
    void Remove${ComponentName}();
}
";
    
    public static string GetStandardComponentInterfaceSource(
        in ComponentData componentData)
    { 
        var componentName = componentData.GetComponentName();
        var validComponentName = componentName.ToLowerFirst().AddPrefixIfIsKeyword();
        var newMethodParameters = componentData.Members.GetMethodParameters(true);

        return StandardComponentInterfaceTemplate
            .Replace("${ComponentName}", componentName)
            .Replace("${ComponentType}", componentData.FullTypeName)
            .Replace("${validComponentName}", validComponentName)
            .Replace("${newMethodParameters}", newMethodParameters);
    }

    const string FlagComponentInterfaceTemplate =
        @"public partial interface I${ComponentName}Entity
{
    bool ${prefixedComponentName} { get; set; }
}
";
    
    public static string GetFlagComponentInterfaceSource(
        in ComponentData componentData)
    {
        return FlagComponentInterfaceTemplate
            .Replace("${ComponentName}", componentData.GetComponentName())
            .Replace("${prefixedComponentName}", componentData.PrefixedComponentName());
    }

    const string EntityComponentInterfaceTemplate = "public partial class ${EntityType} : I${ComponentName}Entity { }\n";

    public static string GetEntityComponentInterfaceSource(
        in ContextData contextData,
        in ComponentData componentData)
    {
        return EntityComponentInterfaceTemplate
            .Replace("${EntityType}", contextData.EntityTypeName)
            .Replace("${ComponentName}", componentData.GetComponentName());
    }

    const string ComponentMatcherApiTemplate =
        @"public sealed partial class ${MatcherType}
{
    static Entitas.IMatcher<${EntityType}> _matcher${ComponentName};

    public static Entitas.IMatcher<${EntityType}> ${ComponentName}
    {
        get
        {
            if (_matcher${ComponentName} == null)
            {
                var matcher = (Entitas.Matcher<${EntityType}>)Entitas.Matcher<${EntityType}>.AllOf(${Index});
                matcher.componentNames = ${componentNames};
                _matcher${ComponentName} = matcher;
            }

            return _matcher${ComponentName};
        }
    }
}
";
    
    public static string GetComponentMatcherApiSource(
        in ContextData contextData,
        in ComponentData componentData)
    {
        var entityType = contextData.EntityTypeName;
        var matcherType = contextData.MatcherTypeName;
        var componentName = componentData.GetComponentName();
        var componentIndex = componentData.GetComponentIndex(contextData);
        var componentNames = $"{contextData.ContextName}{ComponentGenerationHelper.ComponentsLookupName}.componentNames";
        
        return ComponentMatcherApiTemplate
            .Replace("${MatcherType}", matcherType)
            .Replace("${ComponentName}", componentName)
            .Replace("${Index}", componentIndex)
            .Replace("${componentNames}", componentNames)
            .Replace("${EntityType}", entityType);
    }
}