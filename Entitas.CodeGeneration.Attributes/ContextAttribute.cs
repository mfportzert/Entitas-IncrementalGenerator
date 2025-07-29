using System;

namespace Entitas.CodeGeneration.Attributes
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = true)]
    public class ContextAttribute : Attribute
    {
        public readonly string ContextName;

        public ContextAttribute(string contextName)
        {
            ContextName = contextName;
        }
    }
}