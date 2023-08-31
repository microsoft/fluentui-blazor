using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// FluentUI Icon content.
/// </summary>
public class Icon : IconInfo
{
    /// <summary>
    /// Please use the constructor including parameters.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public Icon() : this(string.Empty, IconVariant.Regular, IconSize.Size24, string.Empty)
    { 
        throw new ArgumentNullException("Please use the constructor including parameters.");
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Icon"/> class.
    /// </summary>
    /// <param name="name"><see cref="IconInfo.Name"/></param>
    /// <param name="variant"><see cref="IconInfo.Variant"/></param>
    /// <param name="size"><see cref="IconInfo.Size"/></param>
    /// <param name="content"><see cref="Icon.Content"/></param>
    public Icon(string name, IconVariant variant, IconSize size, string content)
    {
        Name = name;
        Variant = variant;
        Size = size;
        Content = content;
    }

    /// <summary>
    /// Gets the content of the icon: SVG path.
    /// </summary>
    public virtual string Content { get; }

    /// <summary>
    /// Gets the HTML markup of the icon.
    /// </summary>
    public virtual MarkupString ToMarkup(string? size = null, string? color = null)
    {
        var styleWidth = size ?? $"{(int)Size}px";
        var styleColor = color ?? "var(--accent-fill-rest)";
        return new MarkupString($"<svg viewBox=\"0 0 {(int)Size} {(int)Size}\" fill=\"{styleColor}\" style=\"background-color: var(--neutral-layer-1); width: {styleWidth};\" aria-hidden=\"true\">{Content}</svg>");
    }

    /// <summary>
    /// Gets the width of the icon.
    /// </summary>
    protected internal virtual int Width => (int)Size;

    /// <summary>
    /// Returns an icon instance.
    /// </summary>
    /// <returns></returns>
    public static TIcon FromType<TIcon>()
        where TIcon : Icon, new()
    {
        return new TIcon();
    }

    /// <summary>
    /// Returns an icon from an image source.
    /// </summary>
    /// <param name="imageSource"></param>
    /// <returns></returns>
    public static IconFromImage FromImageUrl(string imageSource)
    {
        return new IconFromImage(imageSource);
    }
}
