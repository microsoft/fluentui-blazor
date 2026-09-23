// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Client.Layout.Cookies;

/// <summary>
/// A component that displays a cookie consent banner and manages user preferences for cookies, including analytics,
/// social media, and advertising cookies.
/// </summary>
public partial class CookieConsent()
{
    private const string GA_MEASUREMENT_ID = "G-VML6BZWWTC"; // Google Analytics measurement ID
    private const string MC_PROIOJECT_ID = "hnr14wvzj8";     // Microsoft Clarity project ID

    /// <summary>
    /// Gets or sets the <see cref="IJSRuntime"/> instance used for invoking JavaScript functions from Blazor
    /// components.
    /// </summary>
    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="CookieConsentService"/> instance used for managing cookie consent and preferences.
    /// </summary>
    [Inject]
    public required CookieConsentService CookieConsentService { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="IDialogService"/> instance used for showing dialogs to manage cookie preferences.
    /// </summary>
    [Inject]
    public IDialogService DialogService { get; set; } = default!;

    private bool _showBanner;
    private CookieState? _cookieState;

    private IJSObjectReference? _module;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _cookieState ??= await CookieConsentService.GetCookieStateAsync();
            _showBanner = _cookieState is null;

            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Layout/Cookies/CookieConsent.razor.js");

            if (!_showBanner)
            {
                await InitAnalyticsAsync();
            }

            StateHasChanged();
        }
    }

    private async Task AcceptPolicyAsync()
    {
        _cookieState = new CookieState(true);
        await CookieConsentService.SetCookieStateAsync(_cookieState);
        await InitAnalyticsAsync();

        _showBanner = false;
    }

    private async Task RejectPolicyAsync()
    {
        await CookieConsentService.SetCookieStateAsync(new CookieState(false));

        _showBanner = false;
    }

    /// <summary>
    /// Opens the manage cookies dialog to allow the user to change their cookie preferences. If the dialog is closed
    /// with a result, the cookie state is updated and saved, and analytics are initialized if accepted.
    /// </summary>
    public async Task ManageCookiesAsync()
    {
        _cookieState ??= await CookieConsentService.GetCookieStateAsync() ?? new();

        var result = await DialogService.ShowDialogAsync<ManageCookies>(options =>
        {
            options.Header.Title = $"Manage cookie preferences";
            options.Header.CloseAction.Visible = true;

            options.Parameters.Add(nameof(ManageCookies.Content), _cookieState);
        });

        if (!result.Cancelled && result.Value is not null)
        {
            _cookieState = (CookieState)result.Value;

            await CookieConsentService.SetCookieStateAsync(_cookieState);
            await InitAnalyticsAsync();
        }
    }

    private async Task InitAnalyticsAsync()
    {
        if (_cookieState is not null)
        {
            await CookieConsentService.InitAnalyticsAsync(GA_MEASUREMENT_ID, MC_PROIOJECT_ID, _cookieState.AcceptAnalytics, _cookieState.AcceptAdvertising);
        }
    }
}
