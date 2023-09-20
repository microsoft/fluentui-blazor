// --------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// --------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentGridItem : FluentComponentBase
{
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable SA1300 // Element should begin with upper-case letter

    [Parameter]
    public int? xs { get; set; }

    [Parameter]
    public int? sm { get; set; }

    [Parameter]
    public int? md { get; set; }

    [Parameter]
    public int? lg { get; set; }

    [Parameter]
    public int? xl { get; set; }

    [Parameter]
    public int? xxl { get; set; }

#pragma warning restore SA1300
#pragma warning restore IDE1006

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class).Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style).Build();
}
