// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using FluentUI.Demo.DocApiGen.AssemblyLoading;
using Xunit;

namespace FluentUI.Demo.DocApiGen.Tests.AssemblyLoading;

/// <summary>
/// Regression tests for <see cref="PluginAssemblyLoadContext"/> dependency resolution behaviour.
/// </summary>
public class PluginAssemblyLoadContextTests
{
    // The test project has a project-reference to FluentUI.Demo.DocApiGen, so
    // FluentUI.Demo.DocApiGen.dll is always co-located with this test assembly.
    private const string DepAssemblyName = "FluentUI.Demo.DocApiGen";

    private static string TestOutputDirectory =>
        Path.GetDirectoryName(typeof(PluginAssemblyLoadContextTests).Assembly.Location)!;

    // -----------------------------------------------------------------------
    // Helper
    // -----------------------------------------------------------------------

    /// <summary>
    /// Creates a temporary plugin directory containing only the specified DLL files
    /// (without any .deps.json) so the context falls through to directory probing.
    /// Returns the temp directory path; the caller is responsible for deleting it.
    /// </summary>
    private static string CreateTempPluginDirectory(params string[] dllNames)
    {
        var dir = Path.Combine(Path.GetTempPath(), $"docapigen_plc_test_{Guid.NewGuid():N}");
        Directory.CreateDirectory(dir);

        foreach (var name in dllNames)
        {
            File.Copy(
                Path.Combine(TestOutputDirectory, name),
                Path.Combine(dir, name));
        }

        return dir;
    }

    /// <summary>
    /// Attempts to delete the temporary plugin directory.  Errors are swallowed because
    /// DLL files can remain memory-mapped on Windows; cleanup does not affect assertions.
    /// </summary>
    private static void CleanupContext(PluginAssemblyLoadContext _, string pluginDir)
    {
        try
        {
            Directory.Delete(pluginDir, recursive: true);
        }
        catch (Exception)
        {
            // DLL files may still be memory-mapped on Windows; ignore cleanup errors.
        }
    }

    // -----------------------------------------------------------------------
    // Tests
    // -----------------------------------------------------------------------

    /// <summary>
    /// When a dependency DLL is present in the plugin folder, <see cref="PluginAssemblyLoadContext"/>
    /// must load it from that folder — not from wherever the default context already has it.
    /// </summary>
    [Fact]
    public void Load_WhenDependencyExistsInPluginDirectory_ReturnsAssemblyFromPluginDirectory()
    {
        // Arrange: plugin dir contains both the anchor DLL and the dependency DLL.
        // No .deps.json is present, so the context exercises the directory-probing
        // fallback path in PluginAssemblyLoadContext.Load.
        var pluginDir = CreateTempPluginDirectory(
            "FluentUI.Demo.DocApiGen.Tests.dll",  // anchor (only used to root _pluginDirectory)
            $"{DepAssemblyName}.dll");             // dependency to be resolved from plugin folder

        var anchorPath = Path.Combine(pluginDir, "FluentUI.Demo.DocApiGen.Tests.dll");
        var context = new PluginAssemblyLoadContext(anchorPath);
        try
        {
            // Act: load the dependency by assembly name through the context.
            var loaded = context.LoadFromAssemblyName(new AssemblyName(DepAssemblyName));

            // Assert: the assembly came from the plugin folder, not the default output dir.
            Assert.NotNull(loaded);
            Assert.StartsWith(pluginDir, loaded.Location, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            CleanupContext(context, pluginDir);
        }
    }

    /// <summary>
    /// When a dependency DLL is absent from the plugin folder, <see cref="PluginAssemblyLoadContext"/>
    /// must return <see langword="null"/> from <c>Load</c>, allowing the runtime to fall back to the
    /// default context.  The assembly ultimately returned must come from outside the plugin folder.
    /// </summary>
    [Fact]
    public void Load_WhenDependencyAbsentFromPluginDirectory_DefersToDefaultContext()
    {
        // Arrange: plugin dir contains ONLY the anchor DLL — the dependency is absent.
        var pluginDir = CreateTempPluginDirectory("FluentUI.Demo.DocApiGen.Tests.dll");

        var anchorPath = Path.Combine(pluginDir, "FluentUI.Demo.DocApiGen.Tests.dll");
        var context = new PluginAssemblyLoadContext(anchorPath);
        try
        {
            // Act: the dependency is missing from the plugin dir, so Load returns null and
            // the runtime resolves it through the default AssemblyLoadContext.
            var loaded = context.LoadFromAssemblyName(new AssemblyName(DepAssemblyName));

            // Assert: the resolved assembly is NOT from the (empty) plugin folder.
            Assert.NotNull(loaded);
            Assert.False(
                loaded.Location.StartsWith(pluginDir, StringComparison.OrdinalIgnoreCase),
                $"Assembly should have been resolved from the default context, not the plugin folder. Location: {loaded.Location}");
        }
        finally
        {
            CleanupContext(context, pluginDir);
        }
    }

    /// <summary>
    /// Two separate <see cref="PluginAssemblyLoadContext"/> instances must each load their own
    /// isolated copy of the same assembly, proving context isolation from the default load context.
    /// </summary>
    [Fact]
    public void Load_TwoContextsWithSameDependency_LoadIsolatedInstances()
    {
        var pluginDirA = CreateTempPluginDirectory(
            "FluentUI.Demo.DocApiGen.Tests.dll",
            $"{DepAssemblyName}.dll");
        var pluginDirB = CreateTempPluginDirectory(
            "FluentUI.Demo.DocApiGen.Tests.dll",
            $"{DepAssemblyName}.dll");

        var contextA = new PluginAssemblyLoadContext(Path.Combine(pluginDirA, "FluentUI.Demo.DocApiGen.Tests.dll"));
        var contextB = new PluginAssemblyLoadContext(Path.Combine(pluginDirB, "FluentUI.Demo.DocApiGen.Tests.dll"));
        try
        {
            var assemblyA = contextA.LoadFromAssemblyName(new AssemblyName(DepAssemblyName));
            var assemblyB = contextB.LoadFromAssemblyName(new AssemblyName(DepAssemblyName));

            // Each context must yield a distinct assembly instance.
            Assert.NotSame(assemblyA, assemblyB);

            // And each must come from its own plugin folder.
            Assert.StartsWith(pluginDirA, assemblyA.Location, StringComparison.OrdinalIgnoreCase);
            Assert.StartsWith(pluginDirB, assemblyB.Location, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            CleanupContext(contextA, pluginDirA);
            CleanupContext(contextB, pluginDirB);
        }
    }
}
