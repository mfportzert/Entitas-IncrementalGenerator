using System;

namespace Entitas.CodeGeneration.Attributes
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
    public class ComponentNameAttribute : Attribute
    {
        public readonly string[] ComponentNames;

        public ComponentNameAttribute(params string[] componentNames)
        {
            ComponentNames = componentNames;
        }
    }
}