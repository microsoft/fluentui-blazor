using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Infrastructure;

namespace Microsoft.Fast.Components.FluentUI;

public class StaticAssetService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;
    private NavigationManager _navigationManager;
    private CacheStorageAccessor _cacheStorageAccessor;

    public StaticAssetService(IHttpClientFactory httpClientFactory, NavigationManager navigationManager, CacheStorageAccessor cacheStorageAccessor)
    {
        _navigationManager = navigationManager;

        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient("staticassetservice");
        _httpClient.BaseAddress ??= new Uri(_navigationManager.BaseUri);
        _cacheStorageAccessor = cacheStorageAccessor;
    }

    public async Task<string?> GetAsync(string assetUrl, bool useCache = true)
    {
        string? result = null;
        
        HttpRequestMessage? message = CreateMessage(assetUrl);


        if (useCache)
        {
            // Get the result from the cache
            result = await _cacheStorageAccessor.GetAsync(message);
        }
        
        if (string.IsNullOrEmpty(result))
        {
            //It not in the cache (or cache not used), download the asset
            HttpResponseMessage? response = await _httpClient.SendAsync(message);

            // If successful, store the response in the cache and get the result
            if (response.IsSuccessStatusCode)
            {
                if (useCache)
                    // Store the response in the cache and get the result
                    result = await _cacheStorageAccessor.PutAndGetAsync(message, response);
                else
                    result = await response.Content.ReadAsStringAsync();
            }
            else
                result = string.Empty;
        }

        return result;
    }

    private static HttpRequestMessage CreateMessage(string url) => new(HttpMethod.Get, url);
}
    
