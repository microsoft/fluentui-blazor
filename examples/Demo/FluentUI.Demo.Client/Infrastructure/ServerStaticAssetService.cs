// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Client;

/// <summary />
internal class ServerStaticAssetService : IStaticAssetService
{
    private readonly HttpClient _httpClient;

    /// <summary />
    public ServerStaticAssetService(HttpClient httpClient, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress ??= new Uri(navigationManager.BaseUri);
    }

    /// <summary />
    public async Task<string?> GetAsync(string assetUrl, bool useCache = true)
    {
        var message = new HttpRequestMessage(HttpMethod.Get, assetUrl);
        var response = await _httpClient.SendAsync(message);

        return await response.Content.ReadAsStringAsync();
    }
}
