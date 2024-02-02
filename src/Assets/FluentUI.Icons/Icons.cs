using System.Diagnostics.CodeAnalysis;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public static partial class Icons
{
    private const string Namespace = "Microsoft.FluentUI.AspNetCore.Components";
    private const string LibraryName = "Microsoft.FluentUI.AspNetCore.Components.Icons.dll";

    /// <summary>
    /// Returns a new instance of the icon.
    /// </summary>
    /// <remarks>
    /// This method requires dynamic access to code. This code may be removed by the trimmer.
    /// </remarks>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Raised when the <see cref="IconInfo.Name"/> is not found in predefined icons.</exception>
    [RequiresUnreferencedCode("This method requires dynamic access to code. This code may be removed by the trimmer.")]
    public static CustomIcon GetInstance(IconInfo icon)
    {
        var assembly = AppDomain.CurrentDomain
                                .GetAssemblies()
                                .FirstOrDefault(i => i.ManifestModule.Name == LibraryName);

        if (assembly != null)
        {
            var allIcons = assembly.GetTypes()
                                   .Where(i => i.BaseType == typeof(Icon));

            // Ex. Microsoft.FluentUI.AspNetCore.Components.Icons+Filled+Size10+PresenceAvailable
            var iconFullName = $"{Namespace}.Icons+{icon.Variant}+Size{(int)icon.Size}+{icon.Name}";
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

        throw new ArgumentException($"Icon '{icon.Name}' not found.");
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
        var assembly = AppDomain.CurrentDomain
                                .GetAssemblies()
                                .FirstOrDefault(i => i.ManifestModule.Name == LibraryName);

        if (assembly != null)
        {
            var allTypes = assembly.GetTypes()
                                   .Where(i => i.BaseType == typeof(Icon)
                                            && i.Name != nameof(CustomIcon));

            var allIcons = allTypes.Select(type => Activator.CreateInstance(type) as IconInfo ?? new IconInfo());
            
            return allIcons ?? Array.Empty<IconInfo>();
        }

        return Array.Empty<IconInfo>();
    }

    /// <summary />
    public static IEnumerable<IconInfo> AllIcons
    {
        get
        {
            return GetAllIcons();
        }
    }
}
