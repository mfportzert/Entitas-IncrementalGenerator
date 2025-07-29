using Entitas.CodeGeneration.Components.Data;
using Entitas.CodeGeneration.Extensions;

namespace Entitas.CodeGeneration.Components.Extensions;

public static class ComponentDataExtensions
{
    public static string PrefixedComponentName(this ComponentData data) =>
        data.FlagPrefix.ToLowerFirst() + data.GetComponentName(ComponentGenerationHelper.IgnoreNamespaces);

    public static string ComponentNameValidLowerFirst(this ComponentData data) =>
        data.GetComponentName().ToLowerFirst().AddPrefixIfIsKeyword();
}