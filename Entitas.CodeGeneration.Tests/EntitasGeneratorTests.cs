using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Entitas.CodeGeneration.Tests;

// https://andrewlock.net/creating-a-source-generator-part-2-testing-an-incremental-generator-with-snapshot-testing/
// To run tests in Rider: right-click on this file, and choose "Run Unit Tests"
// or command line (in csproj folder) -> dotnet clean; dotnet build; dotnet test;

[UsesVerify]
public class EntitasGeneratorTests
{
    readonly ITestOutputHelper _output;

    public EntitasGeneratorTests(ITestOutputHelper output)
    {
        _output = output;
    }
    
    [Fact]
    public Task GenerateContext()
    {
        return TestHelper.Verify(TestSources.ContextsTestSource, _output);
    }

    [Fact]
    public Task GenerateComponent()
    {
        return TestHelper.Verify(TestSources.SimpleComponentTestSource, _output);
    }
    
    [Fact]
    public Task IgnoreGeneratedComponents()
    {
        return TestHelper.Verify(TestSources.IgnoreGeneratedComponentsTestSource, _output);
    }
    
    [Fact]
    public Task GenerateFlagPrefixComponent()
    {
        return TestHelper.Verify(TestSources.FlagPrefixTestSource, _output);
    }
    
    [Fact]
    public Task GenerateUniqueComponent()
    {
        return TestHelper.Verify(TestSources.UniqueComponentTestSource, _output);
    }
    
    [Fact]
    public Task GenerateComponentInterface()
    {
        return TestHelper.Verify(TestSources.ComponentInterfaceTestSource, _output);
    }

    [Fact]
    public Task GenerateManyComponents()
    {
        return TestHelper.Verify(TestSources.GenerateManyComponentsTestSource, _output,
            OnSecondCompilation);

        CSharpCompilation OnSecondCompilation(CSharpCompilation compilation)
        {
            var originalTree = compilation.SyntaxTrees[0];

            // Add a new component to ensure the generator:
            // - reuses cached results for existing components
            // - processes only the new one(s)
            var newSyntaxText = originalTree.GetText() + @"
[Game, Audio, Input]
public class TestMember99Component : IComponent
{
    public string Value;
}";
                
            return compilation.ReplaceSyntaxTree(originalTree, CSharpSyntaxTree.ParseText(newSyntaxText));
        }
    }
    
    [Fact]
    public Task GenerateEntityIndexComponents()
    {
        return TestHelper.Verify(TestSources.EntityIndexTestSource, _output);
    }
    
    [Fact]
    public Task GeneratePrimaryEntityIndexComponents()
    {
        return TestHelper.Verify(TestSources.PrimaryEntityIndexTestSource, _output);
    }
    
    [Fact]
    public Task GenerateDestroyEntityCleanupSystem()
    {
        return TestHelper.Verify(TestSources.DestroyComponentCleanupSystemTestSource, _output);
    }
    
    [Fact]
    public Task GenerateRemoveEntityCleanupSystem()
    {
        return TestHelper.Verify(TestSources.RemoveEntityCleanupSystemTestSource, _output);
    }
    
    [Fact]
    public Task GenerateEvents()
    {
        return TestHelper.Verify(TestSources.EventsTestSource, _output);
    }
}