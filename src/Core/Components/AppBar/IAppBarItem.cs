// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Microsoft.FluentUI.AspNetCore.Components;
public interface IAppBarItem
{
    /// <summary>
    /// Gets or sets the unique identifier for this item.
    /// If implementing this interface in a FluentComponentBase derived component, it will be implemented already.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the URL for this item.
    /// </summary>
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets how the link should be matched.
    /// Defaults to <see cref="NavLinkMatch.Prefix"/>.
    /// </summary>
    public NavLinkMatch Match { get; set; }

    /// <summary>
    /// Gets or sets the Icon to use when the item is not hovered/selected/active.
    /// </summary>
    public Icon IconRest { get; set; }

    /// <summary>
    /// Gets or sets the Icon to use when the item is hovered/selected/active.
    /// </summary>
    public Icon? IconActive { get; set; }

    /// <summary>
    /// Gets or sets the text to show under the icon.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the tooltip to show when the item is hovered.
    /// </summary>
    public string? Tooltip { get; set; }

    /// <summary>
    /// Gets or sets the count to show on the item with a <see cref="FluentCounterBadge"/>.
    /// </summary>
    public int? Count { get; set; }

    /// <summary>
    /// Wether this app is outside of visible app bar area.
    /// </summary>
    public bool? Overflow { get; set; }

    /// <summary>
    /// The callback to invoke when the item is clicked.
    /// </summary>
    public EventCallback<IAppBarItem> OnClick { get; set; }

}
