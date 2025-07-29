using System.Runtime.CompilerServices;
using VerifyTests;

namespace Entitas.CodeGeneration.Tests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifySourceGenerators.Enable();
        
        // Generate received/verified .cs files the Snapshots/ folder
        VerifierSettings.DerivePathInfo(
            (sourceFile, projectDirectory, type, method) => new PathInfo(
                directory: Path.Combine(projectDirectory, "Snapshots"),
                typeName: type.Name,
                methodName: method.Name));
    }
}