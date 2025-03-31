// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentToggleButton component allows users to commit a change or trigger a toggle action via a single click or tap and
/// is often found inside forms, dialogs, drawers (panels) and/or pages.
/// </summary>
public partial class FluentMenuButton : FluentButton
{
    private bool EmptyContent => ChildContent is null && Label is null;
}
