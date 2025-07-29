using Entitas.CodeGeneration.Components.Data;
using Entitas.CodeGeneration.Contexts.Data;
using Entitas.CodeGeneration.EntityIndex.Extensions;
using Entitas.CodeGeneration.Extensions;

namespace Entitas.CodeGeneration.EntityIndex;

public static class EntityIndexTemplates
{
    public const string EntityIndexContextsTemplate =
        @"public partial class Contexts
{
${indexConstants}

    [Entitas.CodeGeneration.Attributes.PostConstructor]
    public void InitializeEntityIndices()
    {
${addIndices}
    }
}

public static class ContextsExtensions
{
${getIndices}
}";

    public const string IndexConstantTemplate = @"    public const string ${IndexName} = ""${IndexName}"";";

    const string AddIndexTemplate =
        @"        ${contextName}.AddEntityIndex(new ${IndexType}<${ContextName}Entity, ${KeyType}>(
            ${IndexName},
            ${contextName}.GetGroup(${ContextName}Matcher.${Matcher}),
            (e, c) => ((${ComponentType})c).${MemberName}));";
    
    public static string GetAddIndexSource(
        string indexName,
        in ContextData contextData,
        in ComponentData componentData,
        in MemberData memberData)
    {
        var contextName = contextData.ContextName;
        var contextNameLower = contextName.ToLowerFirst();
        var indexType = memberData.GetEntityIndexType();
        var matcher = indexName;
        
        return AddIndexTemplate
            .Replace("${ContextName}", contextName)
            .Replace("${contextName}", contextNameLower)
            .Replace("${ComponentType}", componentData.FullTypeName)
            .Replace("${IndexName}", indexName)
            .Replace("${MemberName}", memberData.Name)
            .Replace("${KeyType}", memberData.Type)
            .Replace("${IndexType}", indexType)
            .Replace("${Matcher}", matcher);
    }

    // const string ADD_CUSTOM_INDEX_TEMPLATE =
    //     @"        ${contextName}.AddEntityIndex(new ${IndexType}(${contextName}));";

    const string GetIndexTemplate =
        @"    public static System.Collections.Generic.HashSet<${ContextName}Entity> GetEntitiesWith${IndexName}(this ${ContextName}Context context, ${KeyType} ${MemberName}) {
        return ((${IndexType}<${ContextName}Entity, ${KeyType}>)context.GetEntityIndex(Contexts.${IndexName})).GetEntities(${MemberName});
    }";
    
    public static string GetIndexSource(
        string indexName,
        in ContextData contextData,
        in MemberData memberData)
    {
        return GetIndexTemplate
            .Replace("${ContextName}", contextData.ContextName)
            .Replace("${IndexName}", indexName)
            .Replace("${MemberName}", memberData.Name)
            .Replace("${KeyType}", memberData.Type)
            .Replace("${IndexType}", memberData.GetEntityIndexType());
    }

    const string GetPrimaryIndexTemplate =
        @"    public static ${ContextName}Entity GetEntityWith${IndexName}(this ${ContextName}Context context, ${KeyType} ${MemberName}) {
        return ((${IndexType}<${ContextName}Entity, ${KeyType}>)context.GetEntityIndex(Contexts.${IndexName})).GetEntity(${MemberName});
    }";
    
    public static string GetPrimaryIndexSource(
        string indexName,
        in ContextData contextData,
        in MemberData memberData)
    {
        return GetPrimaryIndexTemplate
            .Replace("${ContextName}", contextData.ContextName)
            .Replace("${IndexName}", indexName)
            .Replace("${MemberName}", memberData.Name)
            .Replace("${KeyType}", memberData.Type)
            .Replace("${IndexType}", memberData.GetEntityIndexType());
    }  

//     const string CUSTOM_METHOD_TEMPLATE =
//         @"    public static ${ReturnType} ${MethodName}(this ${ContextName}Context context, ${methodArgs}) {
//         return ((${IndexType})(context.GetEntityIndex(Contexts.${IndexName}))).${MethodName}(${args});
//     }
// ";
}