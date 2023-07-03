namespace Microsoft.Fast.Components.FluentUI;

// If needed, additional services configuration objects can be added here

/// <summary>
/// Defines the global Fluent UI Blazor component library services configuration
/// </summary>
public class LibraryConfiguration
{
    public BlazorHostingModel HostingModel { get; set; } = BlazorHostingModel.NotSpecified;

    public StaticAssetServiceConfiguration StaticAssetServiceConfiguration { get; set; } = new();
    
    public LibraryConfiguration()
    {
    }        
}
