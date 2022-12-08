using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class StaticFileService
{
    public HttpClient HttpClient { get; }
    public NavigationManager NavigationManager { get; }

    public StaticFileService(HttpClient httpClient, NavigationManager navigationManager)
    {
        HttpClient = httpClient;
        NavigationManager = navigationManager;
        HttpClient.BaseAddress ??= new Uri(NavigationManager.BaseUri);
    }

}
