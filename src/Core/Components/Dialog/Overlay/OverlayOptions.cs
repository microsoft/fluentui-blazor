// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Options used to configure the global overlay displayed by the <see cref="FluentDialogProvider"/>.
/// </summary>
public class OverlayOptions
{
    /// <summary>
    /// Gets or sets the text displayed inside the overlay.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Add a card style to the overlay, which includes rounded corners and a shadow.
    /// </summary>
    public CardAppearance? CardAppearance { get; set; }

    /// <summary>
    /// Gets or sets the custom CSS styles applied to the overlay.
    /// </summary>
    public string? Style { get; set; }

    /// <summary>
    /// Gets or sets the custom CSS class applied to the overlay.
    /// </summary>
    public string? Class { get; set; }

    /// <summary />
    internal string? ClassValue => new CssBuilder(Class)
                                        .AddClass("fluent-overlay-global")
                                        .AddClass("fluent-card", when: CardAppearance != null)
                                        .Build();

    /// <summary />
    internal string? StyleValue => new CssBuilder(Style).Build();
}
