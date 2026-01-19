This repository provides a new code generator for <a href="https://github.com/sschmid/Entitas">Entitas</a>, eliminating the need for <a href="https://github.com/sschmid/Jenny">Jenny</a>.

.NET's Incremental Generators perform code generation at compile time, adding source code to your project automatically. You don't need to trigger generations manually anymore.

Support:
- Contexts, components, events, entity index, cleanup systems...
  - Almost everything from the original Entitas code generator (it uses the same generated source templates)
  - Tests are provided in Entitas.CodeGeneration.Tests

Limitations:
- Generated code is not visible in source control.
  - By default, incremental generators inject new source code directly into dlls, at compile time, instead of writing .cs files on disk.
- Not supported: CustomEntityIndex attribute (don't see the need)
- Not supported: Generation of components from non-components classes (when tagging any class with a ContextAttribute).
  - It's quick enough to write a component instead.
- No custom config is passed to the generator:
  - No more Jenny.properties.. so contexts must be defined in code (see Setup section).
  - "Assembly-CSharp" is hardcoded as the main assembly in EntitasGenerator.cs
  - Default Context name is "Game" (when you don't specify the context on a component, it goes to the default context - Game).
- Generation is not supported across multiple assemblies (due to 'partial' usage in Entities and Contexts)

Setup:
- Build the solution (typically in Release configuration)
- Copy <b>Entitas.CodeGeneration.dll</b> and <b>Entitas.CodeGeneration.Attributes.dll</b> in your Unity project.
  - Note: Entitas.CodeGeneration.dll must be in the default assembly (Assembly-CSharp). If you're creating a new project, drop them anywhere you want.
- To allow Entitas.CodeGeneration.dll to work in Unity, follow the documentation: https://docs.unity3d.com/6000.1/Documentation/Manual/create-source-generator.html
    - (In the inspector) Under <b>Select platforms for plugin</b>, disable <b>Any Platform</b>.
    - Under <b>Include Platforms</b>, disable <b>Editor</b> and <b>Standalone</b>.
    - Under <b>Asset Labels</b>, click on the label icon to open the Asset labels sub-menu. Create and assign a new label called <b>RoslynAnalyzer</b>.
- <b>Mandatory:</b> Define at least one context attribute. I recommend writing a 'Game' ContextAttribute (it's the default context), as follow:
```
public sealed class GameAttribute : Entitas.CodeGeneration.Attributes.ContextAttribute
{
    public GameAttribute() : base("Game") {}
}
```

Usage: 
- Write a component as usual
```
public class TestComponent : IComponent
{
    public string Value;
}
```
- After the compilation, you should magically be able to interact with this component on any GameEntity
