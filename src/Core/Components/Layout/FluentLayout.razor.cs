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
    /// 
    /// </summary>
    [Parameter]
    public bool GlobalScrollbar { get; set; } = true;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    internal void AddItem(FluentLayoutItem item)
    {
        Items.Add(item);
    }

    internal string HeaderHeight => Items.FirstOrDefault(i => i.Area == LayoutArea.Header)?.Height ?? "24px";

    internal bool HeaderSticky => Items.FirstOrDefault(i => i.Area == LayoutArea.Header)?.Sticky ?? false;

    internal string FooterHeight => Items.FirstOrDefault(i => i.Area == LayoutArea.Footer)?.Height ?? "24px";

    internal bool FooterSticky => Items.FirstOrDefault(i => i.Area == LayoutArea.Footer)?.Sticky ?? false;
}
