using Entitas.CodeGeneration.Extensions;

namespace Entitas.CodeGeneration.Components.Extensions;

public static class StringExtensions
{
    public static string ToComponentName(this string fullTypeName, bool ignoreNamespaces) => ignoreNamespaces
        ? fullTypeName.ShortTypeName().RemoveComponentSuffix()
        : fullTypeName.RemoveDots().RemoveComponentSuffix();
}