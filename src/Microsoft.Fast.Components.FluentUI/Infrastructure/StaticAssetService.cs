using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class StaticAssetService
{
    public HttpClient HttpClient { get; }
    public NavigationManager NavigationManager { get; }

    public StaticAssetService(HttpClient httpClient, NavigationManager navigationManager)
    {
        HttpClient = httpClient;
        NavigationManager = navigationManager;
        HttpClient.BaseAddress ??= new Uri(NavigationManager.BaseUri);
    }
}
