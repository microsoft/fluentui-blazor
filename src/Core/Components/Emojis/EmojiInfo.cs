namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// FluentUI Emoji meta-data.
/// </summary>
public class EmojiInfo
{
    /// <summary>
    /// Gets the name of the icon: AddCircle, Accessibility, etc.
    /// </summary>
    public virtual string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets the size of the emoji.
    /// </summary>
    public virtual EmojiSize Size { get; init; } = EmojiSize.Size32;

    /// <summary>
    /// Gets the group of the emoji.
    /// </summary>
    public virtual EmojiGroup Group { get; init; } = EmojiGroup.Activities;

    /// <summary>
    /// Gets the skin tone of the emoji.
    /// </summary>
    public virtual EmojiSkintone Skintone { get; init; } = EmojiSkintone.Default;

    /// <summary>
    /// Gets the style of the emoji.
    /// </summary>
    public virtual EmojiStyle Style { get; init; } = EmojiStyle.Color;

    /// <summary>
    /// Gets a list of keywords associated with the emoji (separated by comas).
    /// </summary>
    public virtual string KeyWords { get; init; } = string.Empty;
}
