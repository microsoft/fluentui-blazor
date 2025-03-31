// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Web;

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentSplitButton component allows users to combine a button with a menu where the left part triggers a primary action and the right part opens a menu with secondary actions.
/// is often found inside forms, dialogs, drawers (panels) and/or pages.
/// </summary>
public partial class FluentSplitButton : FluentButton
{
    /// <summary>
    /// Gets or sets the owning FluentMenu.
    /// </summary>
    [CascadingParameter]
    private FluentMenu? Menu { get; set; } = default!;
}
