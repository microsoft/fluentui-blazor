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
public partial class CookieConsent(LibraryConfiguration configuration) : FluentComponentBase(configuration)
{
    private const string JAVASCRIPT_FILE = "./Layout/Cookies/CookieConsent.razor.js";
    private const string GA_MEASUREMENT_ID = "G-VML6BZWWTC"; // Google Analytics measurement ID
    private const string MC_PROJECT_ID = "hnr14wvzj8";     // Microsoft Clarity project ID

    /// <summary>
    /// Gets or sets the <see cref="IDialogService"/> instance used for showing dialogs to manage cookie preferences.
    /// </summary>
    [Inject]
    public IDialogService DialogService { get; set; } = default!;

    private bool _showBanner;
    private CookieState? _cookieState;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Import the JavaScript module
            if (!await JSModule.TryImportJavaScriptModuleAsync(JAVASCRIPT_FILE))
            {
                return;
            }

            _cookieState ??= await GetCookieStateAsync();
            _showBanner = _cookieState is null;

            await InitAnalyticsAsync();

            StateHasChanged();
        }
    }

    private async Task AcceptPolicyAsync()
    {
        _cookieState = new CookieState(true);
        await SetCookieStateAsync(_cookieState);
        await InitAnalyticsAsync();

        _showBanner = false;
    }

    private async Task RejectPolicyAsync()
    {
        _cookieState = new CookieState(false);
        await SetCookieStateAsync(_cookieState);
        await InitAnalyticsAsync();

        _showBanner = false;
    }

    /// <summary>
    /// Opens the manage cookies dialog to allow the user to change their cookie preferences. If the dialog is closed
    /// with a result, the cookie state is updated and saved, and analytics are initialized if accepted.
    /// </summary>
    public async Task ManageCookiesAsync()
    {
        _cookieState ??= await GetCookieStateAsync() ?? new();

        var result = await DialogService.ShowDialogAsync<ManageCookies>(options =>
        {
            options.Header.CloseAction.Visible = true;

            options.Parameters.Add(nameof(ManageCookies.Content), new CookieState(_cookieState.AcceptAnalytics, _cookieState.AcceptSocialMedia, _cookieState.AcceptAdvertising));
        });

        if (!result.Cancelled && result.Value is not null)
        {
            _cookieState = (CookieState)result.Value;

            await SetCookieStateAsync(_cookieState);
            await InitAnalyticsAsync();
        }
    }

    /// <summary>
    /// Retrieves the current cookie state by invoking the JavaScript function "getCookiePolicy" and returns a
    /// <see cref="CookieState"/> object
    /// </summary>
    /// <returns>The current <see cref="CookieState"/> object representing the user's cookie preferences.</returns>
    public async Task<CookieState?> GetCookieStateAsync()
    {
        _cookieState = await JSModule.ObjectReference.InvokeAsync<CookieState?>("getCookiePolicy");

        if (_cookieState != null && _cookieState.AcceptAnalytics == null && _cookieState.AcceptSocialMedia == null && _cookieState.AcceptAdvertising == null)
        {
            _cookieState = null;
        }

        return _cookieState;
    }

    /// <summary>
    /// Sets the cookie state by invoking the JavaScript function "setCookiePolicy" with the provided <see cref="CookieState"/>
    /// object.
    /// </summary>
    /// <param name="state">The <see cref="CookieState"/> object representing the user's cookie preferences.</param>
    /// <returns></returns>
    public async Task SetCookieStateAsync(CookieState state)
    {
        await JSModule.ObjectReference.InvokeVoidAsync("setCookiePolicy", state);
    }

    /// <summary>
    /// Initializes analytics tracking by invoking the JavaScript function "initAnalytics" with the provided Google
    /// Analytics measurement ID and Microsoft Clarity project ID.
    /// </summary>
    public async Task InitAnalyticsAsync()
    {
        if (_cookieState is null)
        {
            _cookieState = new CookieState(false);
        }

        await JSModule.ObjectReference.InvokeVoidAsync("initAnalytics", GA_MEASUREMENT_ID, MC_PROJECT_ID, _cookieState?.AcceptAnalytics, _cookieState?.AcceptAdvertising);
    }
}
