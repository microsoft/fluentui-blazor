// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.DocViewer.Services;

/// <summary>
/// Service to load static assets.
/// </summary>
public class StaticAssetService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="StaticAssetService"/> class.
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="navigationManager"></param>
    public StaticAssetService(HttpClient httpClient, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress ??= new Uri(navigationManager.BaseUri);
    }

    /// <summary>
    /// Load the asset from the given <paramref name="assetUrl"/>.
    /// </summary>
    /// <param name="assetUrl"></param>
    /// <returns></returns>
    public async Task<string?> GetAsync(string assetUrl)
    {
        var message = new HttpRequestMessage(HttpMethod.Get, assetUrl);
        var response = await _httpClient.SendAsync(message);

        return await response.Content.ReadAsStringAsync();
    }
}
