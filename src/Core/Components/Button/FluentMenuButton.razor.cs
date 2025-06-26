// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Web;

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentToggleButton component allows users to commit a change or trigger a toggle action via a single click or tap and
/// is often found inside forms, dialogs, drawers (panels) and/or pages.
/// </summary>
public partial class FluentMenuButton : FluentButton
{
    /// <summary />
    public FluentMenuButton(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary>
    /// Gets or sets the owning FluentMenu.
    /// </summary>
    [CascadingParameter]
    private FluentMenu? Menu { get; set; } = default!;
}
