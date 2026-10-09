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
    private readonly string _generatedId = Identifier.NewId();

    /// <summary />
    public FluentProfileMenu(LibraryConfiguration configuration) : base(configuration)
    {
        Id = _generatedId;
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-profile-menu")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("--fluent-profile-menu-button-size", $"{(int)ButtonSize}px")
        .Build();

    /// <summary />
    protected string? PopoverStyleValue => new StyleBuilder(PopoverStyle)
        .AddStyle("inset-block-start", "calc(var(--fluent-profile-menu-button-size) + 4px) !important", TopCorner)
        .AddStyle("inset-inline-end", "4px !important", TopCorner)
        .AddStyle("inset-inline-start", "auto !important", TopCorner)
        .AddStyle("transform", "none !important", TopCorner)
        .Build();

    /// <summary>
    /// Gets or sets the accessible label of the profile menu trigger and popover.
    /// When omitted, <see cref="FullName"/> or "Profile menu" is used.
    /// </summary>
    [Parameter]
    public string? AriaLabel { get; set; }

    /// <summary>
    /// Gets or sets the size of the avatar used as the profile menu button.
    /// </summary>
    [Parameter]
    public AvatarSize ButtonSize { get; set; } = AvatarSize.Size32;

    /// <summary>
    /// Gets or sets the content displayed in the main section of the popover.
    /// This content replaces the default profile identity.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the email address displayed in the default profile identity.
    /// </summary>
    [Parameter]
    public string? EMail { get; set; }

    /// <summary>
    /// Gets or sets content displayed after the avatar in the profile menu button.
    /// </summary>
    [Parameter]
    public RenderFragment? EndTemplate { get; set; }

    /// <summary>
    /// Gets or sets the footer label displayed at the bottom-left of the popover.
    /// </summary>
    [Parameter]
    public string? FooterLabel { get; set; }

    /// <summary>
    /// Gets or sets the footer action label displayed at the bottom-right of the popover.
    /// </summary>
    [Parameter]
    public string? FooterLink { get; set; }

    /// <summary>
    /// Gets or sets custom content for the footer section of the popover.
    /// This content replaces <see cref="FooterLabel"/> and <see cref="FooterLink"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? FooterTemplate { get; set; }

    /// <summary>
    /// Gets or sets the name displayed in the default profile identity.
    /// </summary>
    [Parameter]
    public string? FullName { get; set; }

    /// <summary>
    /// Gets or sets the header action label displayed at the top-right of the popover.
    /// </summary>
    [Parameter]
    public string? HeaderButton { get; set; }

    /// <summary>
    /// Gets or sets the header label displayed at the top-left of the popover.
    /// </summary>
    [Parameter]
    public string? HeaderLabel { get; set; }

    /// <summary>
    /// Gets or sets custom content for the header section of the popover.
    /// This content replaces <see cref="HeaderLabel"/> and <see cref="HeaderButton"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? HeaderTemplate { get; set; }

    /// <summary>
    /// Gets or sets the image displayed by the profile menu avatars.
    /// </summary>
    [Parameter]
    public string? Image { get; set; }

    /// <summary>
    /// Gets or sets the size of the avatar displayed in the popover.
    /// </summary>
    [Parameter]
    public AvatarSize ImageSize { get; set; } = AvatarSize.Size64;

    /// <summary>
    /// Gets or sets the initials displayed when no image is available.
    /// When omitted, initials are generated from <see cref="FullName"/>.
    /// </summary>
    [Parameter]
    public string? Initials { get; set; }

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
    /// Gets or sets the callback invoked when the footer action is selected.
    /// </summary>
    [Parameter]
    public EventCallback OnFooterLinkClick { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the header action is selected.
    /// </summary>
    [Parameter]
    public EventCallback OnHeaderButtonClick { get; set; }

    /// <summary>
    /// Gets or sets the CSS class applied to the profile popover.
    /// </summary>
    [Parameter]
    public string? PopoverClass { get; set; }

    /// <summary>
    /// Gets or sets the inline style applied to the profile popover.
    /// </summary>
    [Parameter]
    public string? PopoverStyle { get; set; }

    /// <summary>
    /// Gets or sets content displayed before the avatar in the profile menu button.
    /// </summary>
    [Parameter]
    public RenderFragment? StartTemplate { get; set; }

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
    /// Gets or sets a value indicating whether the popover is pinned to the top inline-end corner.
    /// </summary>
    [Parameter]
    public bool TopCorner { get; set; }

    private string FooterLinkLabel => FooterLink ?? Localizer[Localization.LanguageResource.ProfileMenu_ViewAccount];

    private string HeaderButtonLabel => HeaderButton ?? Localizer[Localization.LanguageResource.ProfileMenu_SignOut];

    private string ComponentId => string.IsNullOrWhiteSpace(Id) ? _generatedId : Id;

    private string PopoverId => $"{ComponentId}-popover";

    private string TriggerId => $"{ComponentId}-trigger";

    private string GetAriaLabel()
    {
        if (!string.IsNullOrWhiteSpace(AriaLabel))
        {
            return AriaLabel;
        }

        return !string.IsNullOrWhiteSpace(FullName)
            ? FullName
            : Localizer[Localization.LanguageResource.ProfileMenu_Label];
    }

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
