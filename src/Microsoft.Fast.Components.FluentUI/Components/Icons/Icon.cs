using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// FluentUI Icon content.
/// </summary>
public abstract class Icon : IconInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Icon"/> class.
    /// </summary>
    /// <param name="name"><see cref="Icon.Name"/></param>
    /// <param name="variant"><see cref="Icon.Variant"/></param>
    /// <param name="size"><see cref="Icon.Size"/></param>
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
    public virtual MarkupString Markup
    {
        get
        {
            return new MarkupString($"<svg viewBox=\"0 0 {(int)Size} {(int)Size}\" fill=\"var(--accent-fill-rest)\" style=\"width: {(int)Size}px;\" aria-hidden=\"true\">{Content}</svg>");
        }
    }

    /// <summary>
    /// Gets the width of the icon.
    /// </summary>
    protected internal virtual int Width => (int)Size;
}
