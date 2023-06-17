using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// FluentUI Icon content.
/// </summary>
public abstract class Emoji : EmojiInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Icon"/> class.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="size"></param>
    /// <param name="skintone"></param>
    /// <param name="group"></param>
    /// <param name="style"></param>
    /// <param name="content"></param>
    public Emoji(string name, EmojiSize size, EmojiGroup group, EmojiSkintone skintone, EmojiStyle style, string content)
    {
        Name = name;
        Size = size;
        Skintone = skintone;
        Group = group;
        Style = style;
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
            return new MarkupString($"<svg viewBox=\"0 0 {(int)Size} {(int)Size}\" style=\"width: {(int)Size}px;\" aria-hidden=\"true\">{Content}</svg>");
        }
    }

    /// <summary>
    /// Gets the width of the icon.
    /// </summary>
    protected internal virtual int Width => (int)Size;
}
