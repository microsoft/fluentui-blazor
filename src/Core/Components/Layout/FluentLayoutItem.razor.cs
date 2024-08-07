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
public partial class FluentLayoutItem
{
    private readonly Dictionary<string, string> _extraStyles = new(StringComparer.Ordinal);

    /// <summary>
    /// Gets or sets the Scrollbar Width to compute the correct Aside position.
    /// </summary>
    public static string SCROLLBAR_WIDTH { get; set; } = "14px";

    /// <summary>
    /// <inheritdoc cref="FluentComponentBase.Class"/>
    /// </summary>
    protected string? ClassValue => new CssBuilder(Class)
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
            var styles = new StyleBuilder(Style);

            // Grid Area
            var startAreaName = Area.ToAttributeValue();
            var endAreaName = Area.ToAttributeValue();
            var contentArea = Layout?.Items.FirstOrDefault(i => i.Area == LayoutArea.Content);
            var asideArea = Layout?.Items.FirstOrDefault(i => i.Area == LayoutArea.Aside);

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

            var noChange = string.Equals(startAreaName, endAreaName, StringComparison.CurrentCultureIgnoreCase) || string.IsNullOrEmpty(endAreaName);
            styles.AddStyle("grid-area", noChange
                                       ? startAreaName
                                       //   row-start      / column-start    / row-end       / column-end
                                       : $"{startAreaName} / {startAreaName} / {endAreaName} / {endAreaName}"
                           );

            // Width
            styles.AddStyle("width", Width, when: !string.IsNullOrEmpty(Width));

            // Height
            var height = Area switch
            {
                LayoutArea.Header => Layout?.HeaderHeight ?? "fit-content",
                LayoutArea.Footer => Layout?.FooterHeight ?? "fit-content",
                _ => Height
            };
            styles.AddStyle("height", height, when: !string.IsNullOrEmpty(height));

            // Top when Header is sticky
            var isMiddleArea = Area == LayoutArea.Aside || Area == LayoutArea.Menu || Area == LayoutArea.Content;
            if (isMiddleArea && Layout != null && Layout.HasHeader && Layout.HeaderSticky)
            {
                styles.AddStyle("top", Layout?.HeaderHeight ?? "0");
            }

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
    protected FluentLayout? Layout { get; set; }

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
    /// Default is 24px for Header and Footer, and null for others
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
    override protected void OnInitialized()
    {
        Layout?.AddItem(this);
    }
}
