// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using System.Runtime.Loader;

namespace FluentUI.Demo.DocApiGen;

/// <summary>
/// Loads documentation target assemblies and their dependencies in an isolated context.
/// </summary>
public sealed class DocumentationAssemblyLoadContext : AssemblyLoadContext
{
    private readonly Dictionary<string, string> _inputAssemblyPaths;
    private readonly IReadOnlyList<AssemblyDependencyResolver> _resolvers;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentationAssemblyLoadContext"/> class.
    /// </summary>
    /// <param name="assemblyPaths">The assembly paths that can be loaded as documentation inputs.</param>
    public DocumentationAssemblyLoadContext(IEnumerable<string> assemblyPaths)
        : base(nameof(DocumentationAssemblyLoadContext), isCollectible: false)
    {
        _inputAssemblyPaths = assemblyPaths
            .ToDictionary(
                path => Path.GetFileNameWithoutExtension(path),
                StringComparer.OrdinalIgnoreCase);

#pragma warning disable CA1416 // Validate platform compatibility
        _resolvers = _inputAssemblyPaths.Values
            .Select(path => new AssemblyDependencyResolver(path))
            .ToArray();
#pragma warning restore CA1416 // Validate platform compatibility
    }

    /// <summary>
    /// Resolves managed assemblies for the documentation inputs and their dependencies.
    /// </summary>
    /// <param name="assemblyName">The assembly name to resolve.</param>
    /// <returns>The loaded assembly if resolution succeeds; otherwise, <see langword="null"/>.</returns>
    protected override Assembly? Load(AssemblyName assemblyName)
    {
        if (!string.IsNullOrEmpty(assemblyName.Name) && _inputAssemblyPaths.TryGetValue(assemblyName.Name, out var inputAssemblyPath))
        {
            return LoadFromAssemblyPath(inputAssemblyPath);
        }

        foreach (var resolver in _resolvers)
        {
#pragma warning disable CA1416 // Validate platform compatibility
            var assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);
#pragma warning restore CA1416 // Validate platform compatibility
            if (!string.IsNullOrEmpty(assemblyPath))
            {
                return LoadFromAssemblyPath(assemblyPath);
            }
        }

        return null;
    }

    /// <summary>
    /// Resolves unmanaged libraries for the documentation inputs and their dependencies.
    /// </summary>
    /// <param name="unmanagedDllName">The unmanaged library name to resolve.</param>
    /// <returns>A native library handle if resolution succeeds; otherwise, <c>0</c>.</returns>
    protected override nint LoadUnmanagedDll(string unmanagedDllName)
    {
        foreach (var resolver in _resolvers)
        {
#pragma warning disable CA1416 // Validate platform compatibility
            var unmanagedDllPath = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
#pragma warning restore CA1416 // Validate platform compatibility
            if (!string.IsNullOrEmpty(unmanagedDllPath))
            {
                return LoadUnmanagedDllFromPath(unmanagedDllPath);
            }
        }

        return 0;
    }
}
