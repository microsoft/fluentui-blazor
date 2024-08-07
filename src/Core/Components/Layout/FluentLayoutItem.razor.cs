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
        .AddStyle(GetGridArea())
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
        .AddStyle("height", Height, () => !string.IsNullOrEmpty(Height) && Area != LayoutArea.Header && Area != LayoutArea.Footer)
        .AddStyle("height", Layout?.HeaderHeight ?? "fit-content", () => Area == LayoutArea.Header)
        .AddStyle("height", Layout?.FooterHeight ?? "fit-content", () => Area == LayoutArea.Footer)
        .AddStyle("top", Layout?.HeaderHeight ?? "0", () => Sticky && Layout?.HeaderSticky == true && (Area == LayoutArea.Aside || Area == LayoutArea.Menu || Area == LayoutArea.Content))
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
    /// Default is 24px for Header and Footer, and null for others
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

    private string GetGridArea()
    {
        var firstArea = Area.ToAttributeValue();
        var lastArea = Area.ToAttributeValue();

        var content = Layout?.Items.FirstOrDefault(i => i.Area == LayoutArea.Content);
        var aside = Layout?.Items.FirstOrDefault(i => i.Area == LayoutArea.Aside);

        if (aside != null && Area == LayoutArea.Content)
        {
            if (aside.Sticky)
            {
                lastArea = "aside";
                aside.ExtraStyles = $"margin-right: {(Layout?.GlobalScrollbar == true ? "0" : SCROLLBAR_WIDTH)}";
            }
            else
            {
                lastArea = null;
                aside.ExtraStyles = $"margin-right: 0";
            }

            if (content != null)
            {
                content.ExtraStyles = $"padding-right: {(string.IsNullOrEmpty(aside.Width) || !aside.Sticky ? "0" : aside.Width)}";
            }
        }

        return string.Equals(firstArea, lastArea, StringComparison.CurrentCultureIgnoreCase) || string.IsNullOrEmpty(lastArea)
                ? $"grid-area: {firstArea}"
                : $"grid-area: {firstArea} / {firstArea} / {lastArea} / {lastArea}";    // row-start / column-start / row-end / column-end
    }
}
