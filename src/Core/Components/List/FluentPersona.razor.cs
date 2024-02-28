using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// People picker option component.
/// </summary>
public partial class FluentPersona : FluentComponentBase
{
    /// <summary />
    protected virtual string? ClassValue =>
        new CssBuilder(Class).AddClass("fluent-persona")
                             .Build();

    /// <summary />
    protected virtual string? StyleValue =>
        new StyleBuilder(Style).Build();

    /// <summary>
    /// Gets or sets the initials to display if no image is provided.
    /// By default, the first letters of the <see cref="Name"/> is used.
    /// </summary>
    [Parameter]
    public string? Initials { get; set; }

    /// <summary>
    /// Gets or sets the name to display.
    /// </summary>
    [Parameter]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the content to display under the <see cref="Name"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the image to display, in replacement of the initials.
    /// </summary>
    [Parameter]
    public string? Image { get; set; }

    /// <summary>
    /// Gets or sets the size of the image.
    /// </summary>
    [Parameter]
    public string? ImageSize { get; set; }

    /// <summary>
    /// Gets or sets the status to show. See <see cref="PresenceStatus"/> for options.
    /// </summary>
    [Parameter]
    public PresenceStatus? Status { get; set; }

    /// <summary>
    /// Gets or sets the title to show on hover. If not provided, the status will be used.
    /// </summary>
    [Parameter]
    public string? StatusTitle { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Status"/> size to use.
    /// Default is ExtraSmall.
    /// </summary>
    [Parameter]
    public PresenceBadgeSize StatusSize { get; set; } = PresenceBadgeSize.ExtraSmall;

    /// <summary>
    /// Gets or sets the event raised when the user clicks on the dismiss button.
    /// </summary>
    [Parameter]
    public EventCallback OnDismissClick { get; set; }

    /// <summary>
    /// Gets or sets the title of the dismiss button.
    /// </summary>
    [Parameter]
    public string? DismissTitle { get; set; }

    /// <summary />
    private string GetDefaultInitials()
    {
        var parts = Name.ToUpper().Split(' ');
        return parts == null
                || parts.Length == 0
                || (parts.Length == 1 && parts[0] == string.Empty)
            ? "--"
            : parts.Length > 1
            ? $"{parts[0][0]}{parts[1][0]}"
            : $"{parts[0][0]}";
    }

    private string GetImageSizeStyle()
    {
        return string.IsNullOrEmpty(ImageSize) ? string.Empty : $"width: {ImageSize}; min-width: {ImageSize}; height: {ImageSize}; min-height: {ImageSize};";
    }
}
