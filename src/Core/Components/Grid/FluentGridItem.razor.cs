// --------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// --------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

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

    /// <summary>
    /// Defines how the browser distributes space between and around content items.
    /// </summary>
    [Parameter]
    public JustifyContent? Justify { get; set; }

    /// <summary>
    /// Gets or sets the gaps (gutters) between rows and columns.
    /// See https://developer.mozilla.org/en-US/docs/Web/CSS/gap
    /// </summary>
    [Parameter]
    public string? Gap { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class).Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("justify-content", Justify.ToAttributeValue(), when: Justify is not null)
        .AddStyle("display", "flex", when: Justify is not null)
        .AddStyle("gap", Gap, when: !string.IsNullOrEmpty(Gap))
        .Build();

    /// <summary />
    private bool NoBreakpointsDefined()
    {
        return xs is null
            && sm is null
            && md is null
            && lg is null
            && xl is null
            && xxl is null;
    }
}
