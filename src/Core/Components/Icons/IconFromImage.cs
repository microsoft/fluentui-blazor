namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Icon generated from an image (img src).
/// </summary>
public sealed class IconFromImage : Icon
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IconFromImage"/> class.
    /// </summary>
    public IconFromImage()
        : base(string.Empty, IconVariant.Regular, IconSize.Size24, string.Empty)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="IconFromImage"/> class.
    /// </summary>
    /// <param name="imageUrl">Image source</param>
    public IconFromImage(string imageUrl)
        : base(string.Empty, IconVariant.Regular, IconSize.Size24, $"<img src=\"{imageUrl}\" style=\"width: 100%;\" />")
    { }
}
