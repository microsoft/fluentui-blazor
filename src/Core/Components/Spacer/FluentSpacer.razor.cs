// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Spacer component, used to create space between elements.
/// </summary>
public partial class FluentSpacer : FluentComponentBase
{
    /// <summary />
    public FluentSpacer(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary/>
    public string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary/>
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("flex-grow", "1", when: () => (string.IsNullOrEmpty(Height) && Orientation == Orientation.Vertical) ||
                                                (string.IsNullOrEmpty(Width) && Orientation == Orientation.Horizontal))
        .AddStyle("width", Width, when: () => !string.IsNullOrEmpty(Width) && Orientation == Orientation.Horizontal)
        .AddStyle("height", Height, when: () => !string.IsNullOrEmpty(Height) && Orientation == Orientation.Vertical)
        .Build();

    /// <summary>
    /// Gets or sets the height of the spacer when the <see cref="Orientation"/> is <see cref="Orientation.Vertical"/> (e.g., <c>Height="16px"</c>).
    /// Use <see cref="Width"/> to set the size for a horizontal spacer.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the width of the spacer when the <see cref="Orientation"/> is <see cref="Orientation.Horizontal"/> (e.g., <c>Width="16px"</c>).
    /// Use <see cref="Height"/> to set the size for a vertical spacer.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the orientation of the parent container.
    /// Use <see cref="Orientation.Horizontal"/> (default) for a horizontal spacer, or <see cref="Orientation.Vertical"/> for a vertical one.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;
}
