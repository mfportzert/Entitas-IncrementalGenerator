namespace Entitas.CodeGeneration.Tests;

public static class TestSources
{
    public const string ContextsTestSource = @"
public sealed class GameAttribute : Entitas.CodeGeneration.Attributes.ContextAttribute
{
    public GameAttribute() : base(""Game"") {}
}

public sealed class InputAttribute : Entitas.CodeGeneration.Attributes.ContextAttribute
{
    public InputAttribute() : base(""Input"") {}
}
";
    
    public const string SimpleComponentTestSource = @"
using Entitas;
using Entitas.CodeGeneration.Attributes;

public sealed class GameAttribute : Entitas.CodeGeneration.Attributes.ContextAttribute
{
    public GameAttribute() : base(""Game"") {}
}

public class TestFlagComponent : IComponent
{
}

public class TestMemberComponent : IComponent
{
    public string Value;
}
";
    
    public const string IgnoreGeneratedComponentsTestSource = @"
using Entitas;
using Entitas.CodeGeneration.Attributes;

public sealed class GameAttribute : Entitas.CodeGeneration.Attributes.ContextAttribute
{
    public GameAttribute() : base(""Game"") {}
}

public class NormalComponent : IComponent
{
}

[Entitas.CodeGeneration.Attributes.DontGenerate]
public class GeneratedComponent : IComponent
{
}
";
    
    public const string FlagPrefixTestSource = @"
using Entitas;
using Entitas.CodeGeneration.Attributes;

public sealed class GameAttribute : Entitas.CodeGeneration.Attributes.ContextAttribute
{
    public GameAttribute() : base(""Game"") {}
}

[FlagPrefix(""has"")]
public class TestFlagComponent : IComponent
{
}
";
    
    public const string UniqueComponentTestSource = @"
using Entitas;
using Entitas.CodeGeneration.Attributes;

public sealed class GameAttribute : Entitas.CodeGeneration.Attributes.ContextAttribute
{
    public GameAttribute() : base(""Game"") {}
}

[Unique]
public class TestUniqueComponent : IComponent
{
    public string Value;
}
";
    
    // This test needs a component assigned to 2 contexts
    public const string ComponentInterfaceTestSource = @"
using Entitas;
using Entitas.CodeGeneration.Attributes;

public sealed class GameAttribute : Entitas.CodeGeneration.Attributes.ContextAttribute
{
    public GameAttribute() : base(""Game"") {}
}

public sealed class InputAttribute : Entitas.CodeGeneration.Attributes.ContextAttribute
{
    public InputAttribute() : base(""Input"") {}
}

[Game, Input]
public class TestFlagComponent : IComponent
{
}

[Game, Input]
public class TestMemberComponent : IComponent
{
    public string Value;
}
";
    
    public const string GenerateManyComponentsTestSource = @"
using Entitas;
using Entitas.CodeGeneration.Attributes;

public sealed class GameAttribute : Entitas.CodeGeneration.Attributes.ContextAttribute
{
    public GameAttribute() : base(""Game"") {}
}

public sealed class InputAttribute : Entitas.CodeGeneration.Attributes.ContextAttribute
{
    public InputAttribute() : base(""Input"") {}
}

public sealed class AudioAttribute : Entitas.CodeGeneration.Attributes.ContextAttribute
{
    public AudioAttribute() : base(""Audio"") {}
}

[Game, Audio, Input]
public class TestMemberComponent : IComponent
{
    public string Value;
}
[Game, Audio, Input]
public class TestFlagComponent : IComponent
{
}
[Game, Audio, Input]
public class TestMember3Component : IComponent
{
    public string Value;
}
[Game, Audio, Input]
public class TestMember2Component : IComponent
{
    public string Value;
}
[Game, Audio, Input]
public class TestMember4Component : IComponent
{
    public string Value;
}
[Game, Audio, Input]
public class TestMember5Component : IComponent
{
    public string Value;
}
[Game, Audio, Input]
public class TestMember6Component : IComponent
{
    public string Value;
}
[Game, Audio, Input]
public class TestMember7Component : IComponent
{
    public string Value;
}
[Game, Audio, Input]
public class TestMember8Component : IComponent
{
    public string Value;
}
";
    
    public const string EntityIndexTestSource = @"
using Entitas;
using Entitas.CodeGeneration.Attributes;

public sealed class GameAttribute : Entitas.CodeGeneration.Attributes.ContextAttribute
{
    public GameAttribute() : base(""Game"") {}
}

public class TestMemberComponent : IComponent
{
    [EntityIndex]
    public string Value;
}
";
    
    public const string PrimaryEntityIndexTestSource = @"
using Entitas;
using Entitas.CodeGeneration.Attributes;

public sealed class GameAttribute : Entitas.CodeGeneration.Attributes.ContextAttribute
{
    public GameAttribute() : base(""Game"") {}
}

public class TestMemberComponent : IComponent
{
    [PrimaryEntityIndex]
    public string Value;
}
";
    
    public const string DestroyComponentCleanupSystemTestSource = @"
using Entitas;
using Entitas.CodeGeneration.Attributes;

public sealed class GameAttribute : Entitas.CodeGeneration.Attributes.ContextAttribute
{
    public GameAttribute() : base(""Game"") {}
}

[Cleanup(CleanupMode.DestroyEntity)]
public class TestMemberComponent : IComponent
{
    public string Value;
}
";
    
    public const string RemoveEntityCleanupSystemTestSource = @"
using Entitas;
using Entitas.CodeGeneration.Attributes;

public sealed class GameAttribute : Entitas.CodeGeneration.Attributes.ContextAttribute
{
    public GameAttribute() : base(""Game"") {}
}

[Cleanup(CleanupMode.RemoveComponent)]
public class TestMemberComponent : IComponent
{
    public string Value;
}
";
    
    public const string EventsTestSource = @"
using Entitas;
using Entitas.CodeGeneration.Attributes;

public sealed class GameAttribute : Entitas.CodeGeneration.Attributes.ContextAttribute
{
    public GameAttribute() : base(""Game"") {}
}

public sealed class InputAttribute : Entitas.CodeGeneration.Attributes.ContextAttribute
{
    public InputAttribute() : base(""Input"") {}
}

[Event(EventTarget.Any), Event(EventTarget.Any, EventType.Removed)]
[Event(EventTarget.Self), Event(EventTarget.Self, EventType.Removed)]
public class TestEventComponent : IComponent
{
    public string Value;
}

namespace Namespace
{
    [Event(EventTarget.Any), Event(EventTarget.Any, EventType.Removed)]
    public class TestEvent2Component : IComponent
    {
        public string Value;
    }

    [Game, Input]
    [Event(EventTarget.Any), Event(EventTarget.Any, EventType.Removed)]
    public class TestEvent3Component : IComponent
    {
        public string Value;
    }
}
";
}