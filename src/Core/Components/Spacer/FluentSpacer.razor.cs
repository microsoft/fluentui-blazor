// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Spacer component, used to create space between elements.
/// </summary>
public partial class FluentSpacer : ComponentBase
{
    /// <summary>
    /// Gets or sets the width of the spacer (in pixels).
    /// </summary>
    [Parameter]
    public int? Width { get; set; }
}

