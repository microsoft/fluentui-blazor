namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// FluentUI Icon meta-data.
/// </summary>
public class IconInfo
{
    /// <summary>
    /// Gets the name of the icon: AddCircle, Accessibility, etc.
    /// </summary>
    public virtual string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets the size of the icon: 20, 24, etc.
    /// </summary>
    public virtual IconSize Size { get; init; } = IconSize.Size24;

    /// <summary>
    /// Gets the variant of the icon: Filled, Regular.
    /// </summary>
    public virtual IconVariant Variant { get; init; } = IconVariant.Regular;

    /// <summary>
    /// Returns a new instance of the icon.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Raised when the <see cref="Name"/> is not found in predefined icons.</exception>
    public virtual Icon GetInstance()
    {
        var assembly = typeof(Icon).Assembly;
        var allIcons = assembly.GetTypes().Where(i => i.BaseType == typeof(Icon)).ToArray();

        // Ex. Microsoft.Fast.Components.FluentUI.Icons+Filled+Size10+PresenceAvailable
        var iconFullname = $"{assembly.GetName().Name}+{Variant}+Size{(int)Size}+{Name}";
        var iconType = allIcons.FirstOrDefault(i => i.FullName == iconFullname);

        if (iconType != null)
        {
            return Activator.CreateInstance(iconType) as Icon;
        }
        else 
        { 
            throw new ArgumentException($"Icon '{Name}' not found.");
        }
    }
}