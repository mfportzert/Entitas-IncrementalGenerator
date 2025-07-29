using System;

namespace Entitas.CodeGeneration.Attributes
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
    public class CleanupAttribute : Attribute
    {
        public readonly CleanupMode CleanupMode;

        public CleanupAttribute(CleanupMode cleanupMode)
        {
            CleanupMode = cleanupMode;
        }
    }

    public enum CleanupMode
    {
        RemoveComponent,
        DestroyEntity
    }
}