using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

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
    /// Gets the color of the icon.
    /// </summary>
    internal virtual string? Color { get; private set; }

    /// <summary>
    /// Sets the color of the icon.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public virtual Icon WithColor(string? color)
    {
        if (!string.IsNullOrEmpty(color))
        {
            Color = color;
        }
        return this;
    }

    /// <summary>
    /// Sets the color of the icon.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public virtual Icon WithColor(Color color)
    {
        Color = color.ToAttributeValue();
        return this;
    }

    /// <summary>
    /// Inverse the color of the icon, if the <paramref name="accentContainer"/> is true,
    /// and is not set (<see cref="Color"/> overrides this accent color).
    /// </summary>
    /// <param name="accentContainer"></param>
    /// <returns></returns>
    internal Icon InverseColor(bool accentContainer)
    {
        if (accentContainer && Color == null)
        {
            Color = AspNetCore.Components.Color.Lightweight.ToAttributeValue();
        }

        return this;
    }

    /// <summary>
    /// Gets the HTML markup of the icon.
    /// </summary>
    public virtual MarkupString ToMarkup(string? size = null, string? color = null)
    {
        if (Size != IconSize.Custom && ContainsSVG)
        {
            var styleWidth = size ?? $"{(int)Size}px";
            var styleColor = color ?? Color ?? "var(--accent-fill-rest)";
            return new MarkupString($"<svg viewBox=\"0 0 {(int)Size} {(int)Size}\" fill=\"{styleColor}\" style=\"background-color: var(--neutral-layer-1); width: {styleWidth};\" aria-hidden=\"true\">{Content}</svg>");
        }
        else
        {
            if (string.IsNullOrEmpty(size) && string.IsNullOrEmpty(color))
            {
                return new MarkupString(Content);
            }
            else
            {
                var attributes = new StyleBuilder()
                    .AddStyle("display", "inline-block")
                    .AddStyle("fill", color, when: !string.IsNullOrEmpty(color))
                    .AddStyle("width", size, when: !string.IsNullOrEmpty(size))
                    .Build();

                return new MarkupString($"<div style=\"{attributes}\">{Content}</div>");
            }
        }
    }

    /// <summary>
    /// Gets the width of the icon.
    /// </summary>
    protected internal virtual int Width => Size == IconSize.Custom ? 20 : (int)Size;

    /// <summary>
    /// Returns true if the icon contains a SVG content.
    /// </summary>
    /// <returns></returns>
    protected internal bool ContainsSVG
    {
        get
        {
            return !string.IsNullOrEmpty(Content) &&
                   (Content.StartsWith("<path ") ||
                    Content.StartsWith("<rect ") ||
                    Content.StartsWith("<g ") ||
                    Content.StartsWith("<circle ") ||
                    Content.StartsWith("<mark "));
        }
    }

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
