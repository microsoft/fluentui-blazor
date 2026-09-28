// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Client.Layout.Cookies;

public partial class ManageCookies
{
    /// <summary>
    /// Gets or sets the <see cref="IDialogInstance"/> instance used for managing the dialog state and closing the
    /// dialog.
    /// </summary>
    [CascadingParameter]
    public required IDialogInstance Dialog { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="IDialogService"/> instance used for showing dialogs to manage cookie preferences.
    /// </summary>
    [Inject]
    public required IDialogService DialogService { get; set; }

    private bool _buttonsDisabled => Content.AcceptAnalytics is null && Content.AcceptSocialMedia is null && Content.AcceptAdvertising is null;
    private bool _isResetAllClicked;

    /// <summary>
    /// Gets or sets the <see cref="CookieState"/> instance representing the user's cookie preferences, including
    /// their acceptance of analytics, social media, and advertising cookies.
    /// </summary>
    [Parameter]
    public CookieState Content { get; set; } = default!;

    private async Task HandleSaveAsync()
    {
        await Dialog.CloseAsync(Content);
    }

    private async Task HandleResetAsync()
    {

        Content.AcceptAnalytics = null;
        Content.AcceptSocialMedia = null;
        Content.AcceptAdvertising = null;

        _isResetAllClicked = true;
    }
}
