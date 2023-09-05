using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public static partial class Icons
{
    private const string Namespace = "Microsoft.Fast.Components.FluentUI";
    private const string LibraryName = "Microsoft.Fast.Components.FluentUI.Icons.dll";

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

            // Ex. Microsoft.Fast.Components.FluentUI.Icons+Filled+Size10+PresenceAvailable
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
}
