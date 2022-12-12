using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

<<<<<<<< HEAD:src/Microsoft.Fast.Components.FluentUI/Infrastructure/StaticFileService.cs
public class StaticFileService
========
public class StaticAssetService
>>>>>>>> main:src/Microsoft.Fast.Components.FluentUI/Infrastructure/StaticAssetService.cs
{
    public HttpClient HttpClient { get; }
    public NavigationManager NavigationManager { get; }

<<<<<<<< HEAD:src/Microsoft.Fast.Components.FluentUI/Infrastructure/StaticFileService.cs
    public StaticFileService(HttpClient httpClient, NavigationManager navigationManager)
========
    public StaticAssetService(HttpClient httpClient, NavigationManager navigationManager)
>>>>>>>> main:src/Microsoft.Fast.Components.FluentUI/Infrastructure/StaticAssetService.cs
    {
        HttpClient = httpClient;
        NavigationManager = navigationManager;
        HttpClient.BaseAddress ??= new Uri(NavigationManager.BaseUri);
    }

}
