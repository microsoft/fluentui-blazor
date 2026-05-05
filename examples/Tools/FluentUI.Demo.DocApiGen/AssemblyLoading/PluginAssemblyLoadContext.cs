// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using System.Runtime.Loader;
using System.Runtime.Versioning;

namespace FluentUI.Demo.DocApiGen.AssemblyLoading;

/// <summary>
/// A custom <see cref="AssemblyLoadContext"/> that resolves dependency assemblies
/// from the same directory as the target DLL, preventing version conflicts with
/// assemblies already loaded in the default context.
/// </summary>
[SupportedOSPlatform("windows")]
[SupportedOSPlatform("linux")]
[SupportedOSPlatform("macos")]
public sealed class PluginAssemblyLoadContext : AssemblyLoadContext
{
    private readonly string _pluginDirectory;
    private readonly AssemblyDependencyResolver _resolver;

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginAssemblyLoadContext"/> class.
    /// </summary>
    /// <param name="dllPath">The full path to the target assembly DLL.</param>
    public PluginAssemblyLoadContext(string dllPath)
        : base(name: Path.GetFileNameWithoutExtension(dllPath), isCollectible: true)
    {
        _pluginDirectory = Path.GetDirectoryName(dllPath)!;
        _resolver = new AssemblyDependencyResolver(dllPath);
    }

    /// <inheritdoc/>
    protected override Assembly? Load(AssemblyName assemblyName)
    {
        // First try the dependency resolver (uses the .deps.json next to the DLL)
        var resolvedPath = _resolver.ResolveAssemblyToPath(assemblyName);
        if (resolvedPath != null)
        {
            return LoadFromAssemblyPath(resolvedPath);
        }

        // Fall back to probing the plugin directory directly
        var candidatePath = Path.Combine(_pluginDirectory, assemblyName.Name + ".dll");
        if (File.Exists(candidatePath))
        {
            return LoadFromAssemblyPath(candidatePath);
        }

        // Let the default context handle anything else (BCL, hosting infrastructure, etc.)
        return null;
    }

    /// <inheritdoc/>
    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        var resolvedPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        if (resolvedPath != null)
        {
            return LoadUnmanagedDllFromPath(resolvedPath);
        }

        return IntPtr.Zero;
    }
}
