using System.Collections.Immutable;

namespace Entitas.CodeGeneration.Extensions;

public static class ImmutableArrayExtensions
{
    public static int GetSequenceHashCode<T>(this ImmutableArray<T> array)
    {
        unchecked
        {
            int hash = 17;
            foreach (var item in array)
            {
                hash = hash * 31 + (item?.GetHashCode() ?? 0);
            }
            return hash;
        }
    }
}