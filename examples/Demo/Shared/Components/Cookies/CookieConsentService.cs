// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared.Components.Cookies;

public class CookieConsentService(IJSRuntime jsRuntime) : JSModule(jsRuntime, "./_content/FluentUI.Demo.Shared/Components/Cookies/CookieConsent.razor.js")
{
    private CookieState? _cookieState;

    public async Task<bool> IsAnalyticsConsentedAsync() =>
        (await GetCookieStateAsync())?.AcceptAnalytics == true;

    public async Task<bool> IsSocialMediaConsentedAsync() =>
        (await GetCookieStateAsync())?.AcceptSocialMedia == true;

    public async Task<bool> IsAdvertisingConsentedAsync() =>
        (await GetCookieStateAsync())?.AcceptAdvertising == true;

    public async Task<bool> IsConsentGivenAsync() =>
        await GetCookieStateAsync() != null;

    public async Task<CookieState?> GetCookieStateAsync()
    {
        _cookieState ??= await InvokeAsync<CookieState>("getCookiePolicy");
        return _cookieState;
    }

    public async Task SetCookieStateAsync(CookieState state)
    {
        await InvokeVoidAsync("setCookiePolicy", state);
    }

    public async Task InitAnalyticsAsync(string gaMeasurementID, string mcProjectID)
    {
        await InvokeVoidAsync("initAnalytics", gaMeasurementID, mcProjectID);
    }

}
