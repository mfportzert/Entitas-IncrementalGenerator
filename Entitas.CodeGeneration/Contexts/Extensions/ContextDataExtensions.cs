using System.Collections.Immutable;
using Entitas.CodeGeneration.Contexts.Data;

namespace Entitas.CodeGeneration.Contexts.Extensions;

public static class ContextDataExtensions
{
    public static bool TryGetContext(this ImmutableArray<ContextData> contexts, string name,
        out ContextData result)
    {
        foreach (var context in contexts)
        {
            if (context.ContextName == name)
            {
                result = context;
                return true;
            }
        }
        result = default;
        return false;
    }
}