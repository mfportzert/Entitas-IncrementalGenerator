using System;

namespace Entitas.CodeGeneration.Attributes
{
    /// <summary>
    /// Unsupported by code generation (until really needed)
    /// <br/><br/>
    /// EntityIndex and PrimaryEntityIndex should cover all cases.<br/>
    /// If you want to combine multiple component values into one key:
    ///  - It's possible to use a struct for that
    ///  - Or you can create an extra "UniqueId" component
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class)]
    public class CustomEntityIndexAttribute : Attribute
    {
        public readonly Type ContextType;

        public CustomEntityIndexAttribute(Type contextType)
        {
            ContextType = contextType;
        }
    }
}