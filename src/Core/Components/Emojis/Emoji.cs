using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// FluentUI Emoji content.
/// </summary>
public class Emoji : EmojiInfo
{
    private string? _content = null;

    /// <summary>
    /// Please use the constructor including parameters.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public Emoji() : this(string.Empty, EmojiSize.Size16, EmojiGroup.Flags, EmojiSkintone.Default, EmojiStyle.Flat, new byte[] { })
    {
        throw new ArgumentNullException("Please use the constructor including parameters.");
    }

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
    public virtual string Content => _content ??= EmojiCompress.Unzip(Data);

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
