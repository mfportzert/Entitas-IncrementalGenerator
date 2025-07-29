using Entitas.CodeGeneration.Components.Data;

namespace Entitas.CodeGeneration.EntityIndex.Extensions;

public static class ComponentDataExtensions
{
    public static int GetEntityIndexCount(this ComponentData componentData)
    {
        var count = 0;
        foreach (var member in componentData.Members)
            if (member.IsEntityIndex)
                count++;
        return count;
    }
}