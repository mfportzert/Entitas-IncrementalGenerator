using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Entitas.CodeGeneration.Tests;

// Behavior tests (no snapshots): each one covers a bug that was found by reviewing the generator.
public class GeneratorRegressionTests
{
    // 'Feature' is only generated in the main assembly (visual debugging), so tests declare a stub for it
    const string Header = @"
using Entitas;
using Entitas.CodeGeneration.Attributes;

public class Feature : Entitas.Systems { }

public sealed class GameAttribute : Entitas.CodeGeneration.Attributes.ContextAttribute
{
    public GameAttribute() : base(""Game"") {}
}
";

    sealed class Result
    {
        public List<string> GeneratorWarnings = new();
        public List<string> CompilationErrors = new();
        public List<string> HintNames = new();
        public string AllSources = string.Empty;
    }

    // Runs the generator twice on the same driver, like an IDE while typing: first with 'source',
    // then with 'editedSource' (when given).
    static Result Run(string source, string? editedSource = null)
    {
        var tree = CSharpSyntaxTree.ParseText(Header + source);
        var compilation = CSharpCompilation.Create("Entitas.CodeGeneration-Tests", 
            new[] { tree }, TestHelper.GetCompilationReferences(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new EntitasGenerator());
        driver = driver.RunGenerators(compilation);

        if (editedSource != null)
        {
            var editedTree = CSharpSyntaxTree.ParseText(Header + editedSource);
            compilation = compilation.ReplaceSyntaxTree(tree, editedTree);
            driver = driver.RunGenerators(compilation);
        }

        driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out _);
        var runResult = driver.GetRunResult();

        var result = new Result();
        result.GeneratorWarnings.AddRange(runResult.Diagnostics.Select(d => d.ToString()));
        result.CompilationErrors.AddRange(outputCompilation.GetDiagnostics()
            .Where(d => d.Severity == DiagnosticSeverity.Error)
            .Select(d => $"{d.Id} {d.GetMessage()}"));
        result.HintNames.AddRange(runResult.GeneratedTrees.Select(t => Path.GetFileName(t.FilePath)));
        result.AllSources = string.Join("\n", runResult.GeneratedTrees.Select(t => t.ToString()));
        return result;
    }

    static void AssertHealthy(Result result)
    {
        Assert.Empty(result.GeneratorWarnings);
        Assert.Empty(result.CompilationErrors);
        Assert.NotEmpty(result.HintNames);
    }

    #region Incremental re-run
    // Roslyn keeps the generator state between runs and compares the previous and new values of each step.
    // It crashed there (a default ImmutableArray in the generated event components) when a component
    // changed while another one had an [Event]: that's why the second run is part of this test.
    [Fact]
    public void ChangingAComponent_WhenAnEventComponentExists_DoesNotThrow()
    {
        var before = "[Event(EventTarget.Self)] public class TestEventComponent : IComponent { }";
        var after = "[Event(EventTarget.Self)] public class TestEventComponent : IComponent { public int value; }";

        var result = Run(before, after);
        
        AssertHealthy(result);
    }

    #endregion

    #region Keywords
    // C# keywords can't be used as identifiers: 'event' and 'class' must be written '@event' and '@class'
    [Theory]
    [InlineData("Event")]
    [InlineData("Class")]
    public void ComponentNamedLikeAKeyword_Compiles(string name)
    {
        // the entity property is named after the component, with a lowercase first letter: 'event', 'class'
        var source = $"public class {name}Component : IComponent {{ public int x; }}";

        AssertHealthy(Run(source));
    }

    [Fact]
    public void MemberNamedLikeAKeywordWhenLowered_InEventListener_Compiles()
    {
        // the listener method has a parameter for each member, named with a lowercase first letter: 'int event'
        var source = "[Event(EventTarget.Any)] public class TestEventComponent : IComponent { public int Event; }";

        AssertHealthy(Run(source));
    }

    #endregion

    #region Duplicates

    [Fact]
    public void PartialComponent_IsGeneratedOnce()
    {
        var source = "public partial class PosComponent : IComponent { public int x; } public partial class PosComponent { }";

        var result = Run(source);

        AssertHealthy(result);
        Assert.Equal(1, result.HintNames.Count(n => n == "GamePosComponent.g.cs"));
    }

    [Fact]
    public void PartialComponent_AmongOtherComponents_KeepsTheOthers()
    {
        // the parts of A are not next to each other, and C comes after the duplicate
        var source = "public partial class AComponent : IComponent { public int x; } " +
                     "public class BComponent : IComponent { public int x; } " +
                     "public partial class AComponent { } " +
                     "public class CComponent : IComponent { public int x; }";

        var result = Run(source);

        AssertHealthy(result);
        Assert.Equal(1, result.HintNames.Count(n => n == "GameAComponent.g.cs"));
        Assert.Equal(1, result.HintNames.Count(n => n == "GameBComponent.g.cs"));
        Assert.Equal(1, result.HintNames.Count(n => n == "GameCComponent.g.cs"));
    }

    [Fact]
    public void SameContextTwice_OnAComponent_IsGeneratedOnce()
    {
        var source = "[Game][Context(\"Game\")] public class PosComponent : IComponent { public int x; }";

        AssertHealthy(Run(source));
    }

    [Fact]
    public void SameEventTwice_OnAComponent_IsGeneratedOnce()
    {
        var source = "[Event(EventTarget.Self)][Event(EventTarget.Self)] public class PosComponent : IComponent { public int x; }";

        AssertHealthy(Run(source));
    }

    #endregion

    #region Event names

    [Fact]
    public void EventNames_WhenNamespaceContainsTheComponentName_PrefixOnlyTheComponentName()
    {
        var source = "namespace Camera { [Event(EventTarget.Any)] public class CameraComponent : IComponent { public int x; } }";

        var result = Run(source);

        AssertHealthy(result);
        Assert.Contains("CameraAnyCameraListener", result.AllSources);
        Assert.DoesNotContain("AnyCameraAnyCamera", result.AllSources);
    }

    #endregion
}
