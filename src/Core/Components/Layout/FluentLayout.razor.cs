// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Component that defines a layout for a page, using a grid composed of a Header, a Footer and 3 columns: Menu, Content and Aside Pane.
/// </summary>
public partial class FluentLayout
{
    /// <summary>
    /// Gets the list of items that are part of the layout.
    /// </summary>
    internal List<FluentLayoutItem> Items { get; } = new();

    /// <summary>
    /// Gets or sets the vertical scrollbar position: global to the entire Layout, or inside the content area.
    /// </summary>
    [Parameter]
    public bool GlobalScrollbar { get; set; } = true;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    internal void AddItem(FluentLayoutItem item)
    {
        Items.Add(item);
    }

    /// <summary />
    internal string HeaderHeight => Items.FirstOrDefault(i => i.Area == LayoutArea.Header)?.Height ?? "24px";

    /// <summary />
    internal bool HeaderSticky => Items.FirstOrDefault(i => i.Area == LayoutArea.Header)?.Sticky ?? false;

    /// <summary />
    internal string FooterHeight => Items.FirstOrDefault(i => i.Area == LayoutArea.Footer)?.Height ?? "24px";

    /// <summary />
    internal bool FooterSticky => Items.FirstOrDefault(i => i.Area == LayoutArea.Footer)?.Sticky ?? false;
}
