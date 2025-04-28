// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Area of the layout where the item is placed.
/// </summary>
public partial class FluentLayoutItem : FluentComponentBase
{
    private readonly Dictionary<string, string> _extraStyles = new(StringComparer.Ordinal);

    /// <summary>
    /// Gets or sets the Scrollbar Width to compute the correct Aside position.
    /// </summary>
    public static string SCROLLBAR_WIDTH { get; set; } = "14px";

    /// <summary>
    /// <inheritdoc cref="FluentComponentBase.Class"/>
    /// </summary>
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-layout-item")
        .Build();

    /// <summary>
    /// <inheritdoc cref="FluentComponentBase.Style"/>
    /// </summary>
    protected string? StyleValue
    {
        get
        {
            // User styles
            var styles = DefaultStyleBuilder;

            // Grid Area
            AddGridAreaStyles(styles);

            // Width and Height
            AddWidthHeightStyles(styles);

            // Top when Header is sticky
            AddStickyStyle(styles);

            // Extra styles
            foreach (var item in _extraStyles)
            {
                styles.AddStyle(item.Key, item.Value);
            }

            return styles.Build();
        }
    }

    /// <summary />
    internal void AddExtraStyles(string key, string value) => _extraStyles[key] = value;

    /// <summary>
    /// Gets or sets the parent layout component.
    /// </summary>
    [CascadingParameter]
    private FluentLayout? Layout { get; set; }

    /// <summary>
    /// Gets or sets the type of area where the item is placed.
    /// </summary>
    [Parameter]
    public LayoutArea Area { get; set; } = LayoutArea.Content;

    /// <summary>
    /// Gets ot sets the width of the item.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the item.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets whether the item is sticky.
    /// </summary>
    [Parameter]
    public bool Sticky { get; set; }

    /// <summary>
    /// Gets or sets the contentArea to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    protected override void OnInitialized()
    {
        Layout?.AddItem(this);
    }

    /// <summary>
    /// Add the "grid-area" value
    /// </summary>
    /// <param name="styles"></param>
    private void AddGridAreaStyles(StyleBuilder styles)
    {
        var startAreaName = Area.ToAttributeValue();
        var endAreaName = Area.ToAttributeValue();
        var contentArea = Layout?.Items.FirstOrDefault(i => i.Area == LayoutArea.Content);
        var asideArea = Layout?.Items.FirstOrDefault(i => i.Area == LayoutArea.Aside);

        // Aside
        if (asideArea != null && Area == LayoutArea.Content)
        {
            if (asideArea.Sticky)
            {
                endAreaName = "aside";
                asideArea.AddExtraStyles("margin-right", Layout?.GlobalScrollbar == true ? "0" : SCROLLBAR_WIDTH);
            }
            else
            {
                endAreaName = null;
                asideArea.AddExtraStyles("margin-right", "0");
            }

            contentArea?.AddExtraStyles("padding-right", string.IsNullOrEmpty(asideArea.Width) || !asideArea.Sticky ? "0" : asideArea.Width);
        }

        // Grid Area
        var noChange = string.Equals(startAreaName, endAreaName, StringComparison.CurrentCultureIgnoreCase) || string.IsNullOrEmpty(endAreaName);
        styles.AddStyle("grid-area", noChange
                                   ? startAreaName
                                   //   row-start      / column-start    / row-end       / column-end
                                   : $"{startAreaName} / {startAreaName} / {endAreaName} / {endAreaName}"
                       );
    }

    /// <summary>
    /// Add the "width" and "height" values
    /// </summary>
    /// <param name="styles"></param>
    private void AddWidthHeightStyles(StyleBuilder styles)
    {
        // Width
        styles.AddStyle("width", Width, when: !string.IsNullOrEmpty(Width));

        // Height
        var height = Area switch
        {
            LayoutArea.Header => "var(--layout-header-height)",
            LayoutArea.Footer => "var(--layout-footer-height)",
            LayoutArea.Content => "var(--layout-body-height)",
            _ => Height
        };

        styles.AddStyle("height", height, when: !string.IsNullOrEmpty(height));
    }

    /// <summary>
    /// Add the "top" value when Header is sticky
    /// </summary>
    /// <param name="styles"></param>
    private void AddStickyStyle(StyleBuilder styles)
    {
        var isMiddleArea = Area == LayoutArea.Aside || Area == LayoutArea.Menu || Area == LayoutArea.Content;
        if (isMiddleArea && Layout != null && Layout.HasHeader && Layout.HeaderSticky)
        {
            styles.AddStyle("top", Layout?.HeaderHeight ?? "0");
        }
    }

    /// <inheritdoc />
    public override ValueTask DisposeAsync()
    {
        Layout?.RemoveItem(this);
        return base.DisposeAsync();
    }
}
