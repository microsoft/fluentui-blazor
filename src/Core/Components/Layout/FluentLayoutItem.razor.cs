// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentLayoutItem
{
    /// <summary>
    /// 
    /// </summary>
    public static string SCROLLBAR_WIDTH { get; set; } = "17px";

    /// <summary>
    /// <inheritdoc cref="FluentComponentBase.Class"/>
    /// </summary>
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-layout-item")
        .Build();

    /// <summary>
    /// <inheritdoc cref="FluentComponentBase.Style"/>
    /// </summary>
    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle(GetGridColumnsStyle())
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
        .AddStyle("height", Height, () => !string.IsNullOrEmpty(Height))
        .AddStyle("position", "sticky", () => Sticky)
        .AddStyle(ExtraStyles)
        .Build();

    internal string? ExtraStyles { get; set; }

    /// <summary />
    [CascadingParameter]
    protected FluentLayout? Layout { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public LayoutArea Area { get; set; } = LayoutArea.Content;

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool Sticky { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    override protected void OnInitialized()
    {
        Layout?.AddItem(this);
    }

    private string GetGridColumnsStyle()
    {
        var firstArea = Area.ToAttributeValue();
        var lastArea = Area.ToAttributeValue();

        var aside = Layout?.Items.FirstOrDefault(i => i.Area == LayoutArea.Aside);

        if (aside != null && Area == LayoutArea.Content && aside.Sticky)
        {
            lastArea = "aside";
            aside.ExtraStyles = $"margin-right: {SCROLLBAR_WIDTH};";
        }

        return string.Equals(firstArea, lastArea, StringComparison.CurrentCultureIgnoreCase)
                ? $"grid-area: {firstArea}"
                : $"grid-area: {firstArea} / {firstArea} / {lastArea} / {lastArea}";    // row-start / column-start / row-end / column-end
    }
}
