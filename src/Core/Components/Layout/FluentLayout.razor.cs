// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentLayout
{
    /// <summary>
    /// 
    /// </summary>
    internal List<FluentLayoutItem> Items { get; } = new();

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    [Parameter]
    public RenderFragment? StartTemplate { get; set; }

    /// <summary />
    [Parameter]
    public RenderFragment? EndTemplate { get; set; }

    /// <summary />
    [Parameter]
    public RenderFragment? BodyTemplate { get; set; }

    internal void AddItem(FluentLayoutItem item)
    {
        Items.Add(item);
    }
}
