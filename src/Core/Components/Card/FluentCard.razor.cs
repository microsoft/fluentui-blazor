// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A FluentCard is a container that holds information and actions related to a single concept or object, like a document or a contact.
/// </summary>
public partial class FluentCard : FluentComponentBase
{
    /// <summary />
    public FluentCard(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-card")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("width", Width)
        .AddStyle("height", Height)
        .Build();

    /// <summary>
    /// Gets or sets the content of the card.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the appearance of the component.
    /// </summary>
    [Parameter]
    public CardAppearance? Appearance { get; set; }

    /// <summary>
    /// Gets or sets the shadow of the component.
    /// </summary>
    [Parameter]
    public CardShadow? Shadow { get; set; }

    /// <summary>
    /// Gets or sets the width of the component. 
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the component. 
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Command executed when the user clicks on the card.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Gets or sets the role of the card.
    /// </summary>
    [Parameter]
    public string Role { get; set; } = "group";

    /// <summary />
    internal async Task ClickHandlerAsync(MouseEventArgs args)
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(args);
        }
    }

    /// <summary />
    internal async Task KeyDownHandlerAsync(KeyboardEventArgs args)
    {
        if (string.Equals(args.Key, "Enter", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(args.Key, " ", StringComparison.OrdinalIgnoreCase))
        {
            await ClickHandlerAsync(new MouseEventArgs { ClientX = 0, ClientY = 0 });
        }
    }
}
