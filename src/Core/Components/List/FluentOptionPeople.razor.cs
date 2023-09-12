using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// People picker option component.
/// </summary>
public partial class FluentOptionPeople : FluentComponentBase
{
    /// <summary />
    protected virtual string? ClassValue =>
        new CssBuilder(Class).AddClass("fluent-option-people")
                             .Build();

    /// <summary />
    protected virtual string? StyleValue =>
        new StyleBuilder().AddStyle(Style)
                          .Build();


    /// <summary>
    /// Gets or sets the initials to display if no image is provided.
    /// Byt default, the first letters of the <see cref="Name"/> is used.
    /// </summary>
    [Parameter]
    public string? Initials { get; set; }

    /// <summary>
    /// Gets or sets the name to display.
    /// </summary>
    [Parameter]
    public string Name { get; set; } = string.Empty;

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
}
