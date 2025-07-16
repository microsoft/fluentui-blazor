// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Components.Cookies;

public partial class CookieConsent()
{
    private const string GA_MEASUREMENT_ID = "G-VML6BZWWTC"; // Google Analytics measurement ID
    private const string MC_PROIOJECT_ID = "hnr14wvzj8";     // Microsoft Clarity project ID

    [Inject]
    public required CookieConsentService CookieConsentService { get; set; }

    [Inject]
    public IDialogService DialogService { get; set; } = default!;

    private IDialogReference? _dialog;
    private bool _showBanner = false;
    private CookieState? _cookieState;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _cookieState ??= await CookieConsentService.GetCookieStateAsync();
            _showBanner = _cookieState is null;

            if (!_showBanner && _cookieState?.AcceptAnalytics == true)
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

    public async Task ManageCookiesAsync()
    {
        _cookieState ??= await CookieConsentService.GetCookieStateAsync() ?? new();

        _dialog = await DialogService.ShowDialogAsync(DialogHelper.From<ManageCookies>(), _cookieState, new DialogParameters()
        {
            Title = $"Manage cookie preferences",
            PrimaryAction = "Save changes",
            SecondaryAction = "Reset all",
            ShowDismiss = true,
            Width = "640px",
            Height = "610px",
            PreventDismissOnOverlayClick = true,
            PreventScroll = true,

        });
        var result = await _dialog.Result;

        if (!result.Cancelled && result.Data is not null)
        {
            _cookieState = (CookieState)result.Data;

            await CookieConsentService.SetCookieStateAsync(_cookieState);
            await InitAnalyticsAsync();
        }
    }

    private async Task InitAnalyticsAsync()
    {
        if (_cookieState?.AcceptAnalytics == true)
        {
            await CookieConsentService.InitAnalyticsAsync(GA_MEASUREMENT_ID, MC_PROIOJECT_ID);
        }
    }
}
