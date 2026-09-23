// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.JSInterop;

namespace FluentUI.Demo.Client.Layout.Cookies;

/// <summary>
/// Represents a service for managing cookie consent and preferences in a Blazor application. This service provides
/// methods to check the user's consent for different types of cookies (analytics, social media, advertising), retrieve
/// and set the cookie state, and initialize analytics tracking based on the user's preferences.
/// </summary>
public class CookieConsentService
{
    private const string JAVASCRIPT_FILE = "./Layout/Cookies/CookieConsent.razor.js";

    /// <summary>
    /// Gets or sets the <see cref="IJSRuntime"/> instance used for invoking JavaScript functions related to cookie
    /// consent
    /// </summary>
    public required IJSRuntime JSRuntime { get; set; }

    /// <summary>
    /// Gets or sets the JavaScript module reference used for invoking JavaScript functions related to cookie consent
    /// management.
    /// </summary>
    public required IJSObjectReference? JSModule { get; set; }

    private CookieState? _cookieState;

    /// <summary />
    public CookieConsentService(IJSRuntime JSRuntime)
    {

        this.JSRuntime = JSRuntime;
    }

    /// <summary>
    /// Initializes the JavaScript module by importing the specified JavaScript file using the <see cref="IJSRuntime"/>
    /// instance.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task InitializeAsync()
    {
        JSModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
    }

    /// <summary>
    /// Checks if the user has given consent for analytics cookies by retrieving the current <see cref="CookieState"/>
    /// </summary>
    /// <returns>A boolean value indicating whether the user has given consent for analytics cookies.</returns>
    public async Task<bool> IsAnalyticsConsentedAsync() =>
        (await GetCookieStateAsync())?.AcceptAnalytics == true;

    /// <summary>
    /// Checks if the user has given consent for social media cookies by retrieving the current
    /// <see cref="CookieState"/>
    /// </summary>
    /// <returns>A boolean value indicating whether the user has given consent for social media cookies.</returns>
    public async Task<bool> IsSocialMediaConsentedAsync() =>
        (await GetCookieStateAsync())?.AcceptSocialMedia == true;

    /// <summary>
    /// Checks if the user has given consent for advertising cookies by retrieving the current <see cref="CookieState"/>
    /// and verifying if
    /// </summary>
    /// <returns>A boolean value indicating whether the user has given consent for advertising cookies.</returns>
    public async Task<bool> IsAdvertisingConsentedAsync() =>
        (await GetCookieStateAsync())?.AcceptAdvertising == true;

    /// <summary>
    /// Checks if the user has given consent for any cookies by retrieving the current <see cref="CookieState"/> and verifying if it
    /// is not null.
    /// </summary>
    /// <returns>A boolean value indicating whether the user has given consent for any cookies.</returns>
    public async Task<bool> IsConsentGivenAsync() =>
        await GetCookieStateAsync() != null;

    /// <summary>
    /// Retrieves the current cookie state by invoking the JavaScript function "getCookiePolicy" and returns a
    /// <see cref="CookieState"/> object
    /// </summary>
    /// <returns>The current <see cref="CookieState"/> object representing the user's cookie preferences.</returns>
    public async Task<CookieState?> GetCookieStateAsync()
    {
        await InitializeAsync();

        _cookieState = await JSModule!.InvokeAsync<CookieState?>("getCookiePolicy");

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
        await InitializeAsync();
        await JSModule!.InvokeVoidAsync("setCookiePolicy", state);
    }

    /// <summary>
    /// Initializes analytics tracking by invoking the JavaScript function "initAnalytics" with the provided Google
    /// Analytics measurement ID and Microsoft Clarity project ID.
    /// </summary>
    /// <param name="gaMeasurementID">The Google Analytics measurement ID.</param>
    /// <param name="mcProjectID">The Microsoft Clarity project ID.</param>
    /// <param name="acceptAnalytics">A boolean value indicating whether the user has accepted analytics cookies.</param>
    /// <param name="acceptAdvertising">A boolean value indicating whether the user has accepted advertising cookies.</param>
    /// <returns></returns>
    public async Task InitAnalyticsAsync(string gaMeasurementID, string mcProjectID, bool? acceptAnalytics, bool? acceptAdvertising)
    {
        await InitializeAsync();
        await JSModule!.InvokeVoidAsync("initAnalytics", gaMeasurementID, mcProjectID, acceptAnalytics, acceptAdvertising);
    }
}
