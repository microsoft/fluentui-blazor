// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentButton component allows users to commit a change or trigger an action via a single click or tap and is often found inside forms, dialogs, drawers (panels) and/or pages.
/// </summary>
public partial class FluentAvatar : FluentComponentBase
{
    /// <summary />
    protected string? ClassValue => DefaultClassBuilder.Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("cursor", "pointer", when: OnClick.HasDelegate)
        .Build();

    /// <summary>
    /// Gets or sets activity indicator
    /// </summary>
    [Parameter]
    public bool? Active { get; set; }

    /// <summary>
    /// Gets or sets the styled appearance of the avatar when the avatar is "active".
    /// </summary>
    [Parameter]
    public AvatarActiveAppearance? ActiveAppearance { get; set; }

    /// <summary>
    /// Gets or sets the color of the avatar.
    /// </summary>
    [Parameter]
    public AvatarColor? Color { get; set; }

    /// <summary>
    /// Gets or sets the default icon. The icon will only be shown when there is no image or initials available.
    /// </summary> 
    [Parameter]
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets an avatar can display an image.
    /// </summary>
    [Parameter]
    public string? Image { get; set; }

    /// <summary>
    /// Gets or sets custom initials rather than one generated via the name
    /// </summary>
    [Parameter]
    public string? Initials { get; set; }

    /// <summary>
    /// The name of the person or entity represented by this Avatar. This should always be provided if it is available.
    /// The name is used to determine the initials displayed when there is no image.It is also provided to accessibility tools.
    /// </summary>
    [Parameter]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the shape of the avatar.
    /// </summary>
    [Parameter]
    public AvatarShape? Shape { get; set; }

    /// <summary>
    /// Gets or sets size of the avatar.
    /// </summary>
    [Parameter]
    public AvatarSize? Size { get; set; }

    /// <summary>
    /// Gets or sets a command executed when the user clicks on the button.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary />
    protected async Task OnClickHandlerAsync(MouseEventArgs e)
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(e);
        }
    }

    /// <summary />
    protected async Task OnKeyDownHandlerAsync(KeyboardEventArgs e)
    {
        if (string.Equals(e.Code, "Enter", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(e.Code, "Space", StringComparison.OrdinalIgnoreCase))
        {
            await OnClickHandlerAsync(new MouseEventArgs());
        }
    }

    private string? GetInitialsValue =>
        string.IsNullOrEmpty(Initials)
        ? null
        : Initials;

    private string? GetNameValue =>
        string.IsNullOrEmpty(Name)
        ? null
        : Name;

    private bool DisplayIcon =>
        Icon is not null
        && string.IsNullOrEmpty(Name)
        && string.IsNullOrEmpty(Initials);

    private bool DisplayImage => !string.IsNullOrEmpty(Image);

    private string GetAvatarSize =>
        Size is not null
        ? $"{(int)Size}px"
        : "32px"; // Default component size

    private string? GetActiveValue =>
        Active.HasValue
        ? Active.Value
            ? "active"
            : "inactive"
        : null;

    private string? GetRoleValue =>
        OnClick.HasDelegate
        ? "button"
        : null;

    private int? GetTabIndexValue =>
        OnClick.HasDelegate && (!AdditionalAttributes?.ContainsKey("tabindex") ?? true)
        ? 0
        : null;
}
