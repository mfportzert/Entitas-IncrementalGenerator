namespace Entitas.CodeGeneration.ComponentsLookups;

public static class ComponentsLookupTemplates
{
    public const string ComponentsLookupTemplate =
        @"public static class ${Lookup}
{
${componentConstantsList}

${totalComponentsConstant}

    public static readonly string[] componentNames = 
    {
${componentNamesList}
    };

    public static readonly System.Type[] componentTypes = 
    {
${componentTypesList}
    };
}
";
    
    public const string ComponentConstantTemplate = @"    public const int ${ComponentName} = ${Index};";
    public const string TotalComponentsConstantTemplate = @"    public const int TotalComponents = ${totalComponents};";
    public const string ComponentNameTemplate = @"        ""${ComponentName}""";
    public const string ComponentTypeTemplate = @"        typeof(${ComponentType})";
}