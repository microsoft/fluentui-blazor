// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Displays a user avatar that opens a profile summary and related account actions.
/// </summary>
public partial class FluentProfileMenu : FluentComponentBase
{
    /// <summary />
    public FluentProfileMenu(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-profile-menu")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    /// Gets or sets the image displayed by the profile menu avatars.
    /// </summary>
    [Parameter]
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the initials displayed when no image is available.
    /// </summary>
    [Parameter]
    public string? ImageInitials { get; set; }

    /// <summary>
    /// Gets or sets the size of the avatar used as the profile menu button.
    /// </summary>
    [Parameter]
    public AvatarSize? ImageSize { get; set; }

    /// <summary>
    /// Gets or sets content displayed before the avatar in the profile menu button.
    /// </summary>
    [Parameter]
    public RenderFragment? StartTemplate { get; set; }

    /// <summary>
    /// Gets or sets the content displayed in the main section of the popover.
    /// This content replaces the default profile identity.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets content displayed after the avatar in the profile menu button.
    /// </summary>
    [Parameter]
    public RenderFragment? EndTemplate { get; set; }

    /// <summary>
    /// Gets or sets the title of the profile menu.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the profile menu is open.
    /// </summary>
    [Parameter]
    public bool Open { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when <see cref="Open"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OpenChanged { get; set; }

    /// <summary>
    /// Gets or sets the presence status displayed on the profile menu button.
    /// </summary>
    [Parameter]
    public PresenceStatus? Status { get; set; }

    /// <summary>
    /// Gets or sets the presence status tooltip.
    /// </summary>
    [Parameter]
    public string? StatusTitle { get; set; }

    /// <summary>
    /// Gets or sets the size of the presence status badge.
    /// </summary>
    [Parameter]
    public BadgeSize? StatusSize { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the popover is pinned to the top inline-end corner.
    /// </summary>
    [Parameter]
    public bool TopCorner { get; set; }

    // private string FooterLinkLabel => FooterLink ?? Localizer[Localization.LanguageResource.ProfileMenu_ViewAccount];

    // private string HeaderButtonLabel => HeaderButton ?? Localizer[Localization.LanguageResource.ProfileMenu_SignOut];

    // private string ComponentId => string.IsNullOrWhiteSpace(Id) ? _generatedId : Id;

    private string PopoverId => $"{Id}-popover";

    private string TriggerId => $"{Id}-trigger";

    private string GetStatusTitle()
    {
        if (!string.IsNullOrWhiteSpace(StatusTitle))
        {
            return StatusTitle;
        }

        return Status switch
        {
            PresenceStatus.Available => Localizer[Localization.LanguageResource.PresenceStatus_Available],
            PresenceStatus.Busy => Localizer[Localization.LanguageResource.PresenceStatus_Busy],
            PresenceStatus.Away => Localizer[Localization.LanguageResource.PresenceStatus_Away],
            PresenceStatus.OutOfOffice => Localizer[Localization.LanguageResource.PresenceStatus_OutOfOffice],
            PresenceStatus.Offline => Localizer[Localization.LanguageResource.PresenceStatus_Offline],
            PresenceStatus.DoNotDisturb => Localizer[Localization.LanguageResource.PresenceStatus_DoNotDisturb],
            PresenceStatus.Blocked => Localizer[Localization.LanguageResource.PresenceStatus_Blocked],
            _ => Localizer[Localization.LanguageResource.PresenceStatus_Unknown],
        };
    }

    private async Task HandleKeyDownAsync(KeyboardEventArgs args)
    {
        if (Open && string.Equals(args.Key, "Escape", StringComparison.Ordinal))
        {
            Open = false;
            await OpenChanged.InvokeAsync(Open);
        }
    }

    private async Task HandleOpenChangedAsync(bool open)
    {
        if (Open == open)
        {
            return;
        }

        Open = open;
        await OpenChanged.InvokeAsync(Open);
    }

    private async Task ToggleAsync()
    {
        Open = !Open;
        await OpenChanged.InvokeAsync(Open);
    }
}
