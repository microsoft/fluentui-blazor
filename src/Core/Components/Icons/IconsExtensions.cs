// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public static partial class IconsExtensions
{
    private const string Namespace = "Microsoft.FluentUI.AspNetCore.Components";
    private const string LibraryName = "Microsoft.FluentUI.AspNetCore.Components.Icons.{0}";    // {0} must be replaced with the "Variant": Regular, Filled, etc.

    /// <summary>
    /// Returns a new instance of the icon.
    /// </summary>
    /// <param name="icon">The <see cref="IconInfo"/> to instantiate.</param>
    /// <remarks>
    /// This method requires dynamic access to code. This code may be removed by the trimmer.
    /// If the assembly is not yet loaded, it will be loaded by the method `Assembly.Load`.
    /// To avoid any issues, the assembly must be loaded before calling this method (e.g. adding an icon in your code).
    /// </remarks>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Raised when the <see cref="IconInfo.Name"/> is not found in predefined icons.</exception>
    [RequiresUnreferencedCode("This method requires dynamic access to code. This code may be removed by the trimmer.")]
    public static CustomIcon GetInstance(this IconInfo icon)
    {
        var assemblyName = string.Format(LibraryName, icon.Variant);
        var assembly = GetAssembly(assemblyName);

        if (assembly != null)
        {
            var allIcons = assembly.GetTypes()
                                   .Where(i => i.BaseType == typeof(Icon));

            // Ex. Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size10+PresenceAvailable
            var iconFullName = $"{Namespace}.Icons.{icon.Variant}.Size{(int)icon.Size}+{icon.Name}";
            var iconType = allIcons.FirstOrDefault(i => i.FullName == iconFullName);

            if (iconType != null)
            {
                var newIcon = Activator.CreateInstance(iconType);
                if (newIcon != null)
                {
                    return new CustomIcon((Icon)newIcon);
                }
            }
        }

        throw new ArgumentException($"Icon 'Icons.{icon.Variant}.Size{(int)icon.Size}.{icon.Name}' not found.");
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
    public static IEnumerable<IconInfo> GetAllIcons()
    {
        var allIcons = new List<IconInfo>();

        foreach (var variant in Enum.GetValues(typeof(IconVariant)).Cast<IconVariant>())
        {
            var assemblyName = string.Format(LibraryName, variant);
            var assembly = GetAssembly(assemblyName);

            if (assembly != null)
            {
                var allTypes = assembly.GetTypes()
                                       .Where(i => i.BaseType == typeof(Icon)
                                                && i.Name != nameof(CustomIcon));

                allIcons.AddRange(allTypes.Select(type => Activator.CreateInstance(type) as IconInfo ?? new IconInfo()));
            }
        }

        return allIcons;
    }

    /// <summary />
    public static IEnumerable<IconInfo> AllIcons
    {
        get
        {
            return GetAllIcons();
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
