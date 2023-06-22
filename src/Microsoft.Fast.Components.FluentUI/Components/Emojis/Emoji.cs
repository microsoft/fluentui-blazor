using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// FluentUI Icon content.
/// </summary>
public abstract class Emoji : EmojiInfo
{
    private string? _content = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="Icon"/> class.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="size"></param>
    /// <param name="skintone"></param>
    /// <param name="group"></param>
    /// <param name="style"></param>
    /// <param name="data"></param>
    public Emoji(string name, EmojiSize size, EmojiGroup group, EmojiSkintone skintone, EmojiStyle style, byte[] data)
    {
        Name = name;
        Size = size;
        Skintone = skintone;
        Group = group;
        Style = style;
        Data = data; // Zipped SVG content 
    }

    private byte[] Data { get; }

    /// <summary>
    /// Gets the content of the icon: SVG path.
    /// </summary>
    public virtual string Content => _content ?? (_content = EmojiCompress.Unzip(Data));

    /// <summary>
    /// Gets the HTML markup of the emoji.
    /// </summary>
    public virtual MarkupString ToMarkup(string? size = null)
    {
        var styleWidth = size ?? $"{(int)Size}px";
        return new MarkupString($"<svg viewBox=\"0 0 {(int)Size} {(int)Size}\" style=\"width: {styleWidth};\" aria-hidden=\"true\">{Content}</svg>");
    }

    /// <summary>
    /// Gets the width of the icon.
    /// </summary>
    protected internal virtual int Width => (int)Size;
}
