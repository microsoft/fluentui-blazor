namespace Microsoft.FluentUI.AspNetCore.Components;

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
}
