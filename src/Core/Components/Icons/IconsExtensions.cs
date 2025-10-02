// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public static partial class IconsExtensions
{
    private const string LibraryName = "Microsoft.FluentUI.AspNetCore.Components.Icons.{0}";    // {0} must be replaced with the "Variant": Regular, Filled, etc.
    private const string CoreIconsLibraryName = "Microsoft.FluentUI.AspNetCore.Components";

    /// <summary>
    /// Returns a new instance of the icon.
    /// </summary>
    /// <param name="icon">The <see cref="IconInfo"/> to instantiate.</param>
    /// <param name="throwOnError">true to throw an exception if the type is not found (default); false to return null.</param>
    /// <remarks>
    /// This method requires dynamic access to code. This code may be removed by the trimmer.
    /// If the assembly is not yet loaded, it will be loaded by the method `Assembly.Load`.
    /// To avoid any issues, the assembly must be loaded before calling this method (e.g. adding an icon in your code).
    /// </remarks>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Raised when the <see cref="IconInfo.Name"/> is not found in predefined icons.</exception>
    [RequiresUnreferencedCode("This method requires dynamic access to code. This code may be removed by the trimmer.")]
    public static CustomIcon GetInstance(this IconInfo icon, bool? throwOnError = true)
    {
        var assembly = GetAssembly(icon.AssemblyName);

        if (assembly != null)
        {
            var allIcons = assembly.GetTypes()
                                   .Where(i => i.BaseType == typeof(Icon));

            // Ex. Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size10+PresenceAvailable
            var iconType = allIcons.FirstOrDefault(i => i.FullName == icon.FullName);

            if (iconType != null)
            {
                var newIcon = Activator.CreateInstance(iconType);
                if (newIcon != null)
                {
                    return new CustomIcon((Icon)newIcon);
                }
            }
        }

        if (throwOnError == true || throwOnError == null)
        {
            throw new ArgumentException($"Icon 'Icons.{icon.Variant}.Size{(int)icon.Size}.{icon.Name}' not found.");
        }

        return default!;
    }

    /// <summary>
    /// Tries to return a new instance of the icon.
    /// </summary>
    /// <param name="icon">The <see cref="IconInfo"/> to instantiate.</param>
    /// <param name="result">When this method returns, contains the <see cref="CustomIcon"/> value if the conversion succeeded, or null if the conversion failed. This parameter is passed uninitialized; any value originally supplied in result will be overwritten.</param>
    /// <remarks>
    /// This method requires dynamic access to code. This code may be removed by the trimmer.
    /// If the assembly is not yet loaded, it will be loaded by the method `Assembly.Load`.
    /// To avoid any issues, the assembly must be loaded before calling this method (e.g. adding an icon in your code).
    /// </remarks>
    /// <returns>True if the icon was found and created; otherwise, false.</returns>
    [RequiresUnreferencedCode("This method requires dynamic access to code. This code may be removed by the trimmer.")]
    public static bool TryGetInstance(this IconInfo icon, out CustomIcon? result)
    {
        result = GetInstance(icon, throwOnError: false);
        return result != null;
    }

    /// <summary>
    /// Returns a new instance of the icon.
    /// </summary>
    /// <remarks>
    /// This method requires dynamic access to code. This code may be removed by the trimmer.
    /// </remarks>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Raised when the <see cref="IconInfo.Name"/> is not found in predefined icons.</exception>
    [RequiresUnreferencedCode("This method requires dynamic access to code. This code may be removed by the trimmer.")]
    public static IEnumerable<IconInfo> GetAllIcons(string assemblyName)
    {
        var assembly = GetAssembly(assemblyName);
        if (assembly is null)
        {
            return Array.Empty<IconInfo>();
        }
        var allTypes = assembly.GetTypes()
                               .Where(i => i.BaseType == typeof(Icon)
                                        && i.Name != nameof(CustomIcon));

        return allTypes.Select(type => Activator.CreateInstance(type) as IconInfo ?? new IconInfo());
    }

    /// <summary />
    public static IEnumerable<IconInfo> AllIcons
    {
        get
        {
            var allIcons = new List<IconInfo>();
            foreach (var variant in Enum.GetValues(typeof(IconVariant)).Cast<IconVariant>())
            {
                var assemblyName = string.Format(LibraryName, variant);
                allIcons.AddRange(GetAllIcons(assemblyName));
            }
            return allIcons;
        }
    }

    public static IEnumerable<IconInfo> CoreIcons
    {
        get
        {
            return GetAllIcons(CoreIconsLibraryName);
        }
    }

    /// <summary />
    private static Assembly? GetAssembly(string assemblyName)
    {
        try
        {
            return AppDomain.CurrentDomain
                            .GetAssemblies()
                            .FirstOrDefault(i => i.ManifestModule.Name == assemblyName + ".dll")
                ?? Assembly.Load(assemblyName);

        }
        catch (Exception)
        {
            return null;
        }
    }
}
