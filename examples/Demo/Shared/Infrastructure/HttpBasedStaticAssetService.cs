using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Shared;

public class HttpBasedStaticAssetService : IStaticAssetService
{
    private readonly HttpClient _httpClient;
    private readonly CacheStorageAccessor _cacheStorageAccessor;

    public HttpBasedStaticAssetService(HttpClient httpClient, NavigationManager navigationManager, CacheStorageAccessor cacheStorageAccessor)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress ??= new Uri(navigationManager.BaseUri);
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
                {
                    // Store the response in the cache and get the result
                    result = await _cacheStorageAccessor.PutAndGetAsync(message, response);
                }
                else
                {
                    result = await response.Content.ReadAsStringAsync();
                }
            }
            else
            {
                result = string.Empty;
            }
        }

        return result;
    }

    private static HttpRequestMessage CreateMessage(string url) => new(HttpMethod.Get, url);
}

